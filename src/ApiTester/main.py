import os
import sys
import argparse
import traceback
import logging
from exceptions import ApiException
from pprint import pprint
from transform import transform
from command import Command
from ui import printError
from ui import printPrompt
from session import Session
from config import Config
from property_bag import PropertyBag
from transform import transformValue
from publish.publisher import Publisher

publisher = Publisher({})

def read_variables(file_name):
    variables = {}
    with open(file_name, 'r') as in_file:
        for line in in_file.readlines():
            parts = line.strip().split('=')
            if len(parts) > 1:
                if parts[0].startswith('#') == False:
                    variables[parts[0]] = transformValue(parts[1], variables)
    return variables


def main():
    # Load file
    parser = argparse.ArgumentParser()
    parser.add_argument("--batch", help="Batch file name")
    parser.add_argument("--config", help="Config File")
    parser.add_argument("--varfile", help="Variables file")
    parser.add_argument("--session", help="Session name")
    parser.add_argument("--log_level", help="Loglevel(DEBUG,INFO,ERROR)")
    parser.add_argument("--output", help="Output directory where results will be written")
    parser.add_argument("--resource_path", help="Resource Path")
    parser.add_argument("--readonly", help="If set to set true will allow only GET operations")
    args, unknown = parser.parse_known_args()

    if args.config == None:
        printError('Config file is required . [ex: main.py --config apigee.json] ')
        parser.print_help()
        sys.exit(-1)
    
    # Set log level.
    if args.log_level == None:
        logging.basicConfig(stream=sys.stderr, level=logging.ERROR)
    else:
        level = logging._nameToLevel.get(args.log_level,None)
        if level != None:
            logging.basicConfig(stream=sys.stderr, level=level)


    # convert unknowns
    items = {x.split('=')[0][2:]: x.split('=')[-1] for x in unknown if x[:2] == '--'}
    commandParameters = dict(args.__dict__, **items)
        
    # Load variables
    variables = {}
    if args.varfile != None:
        for variable_file in args.varfile.split(','):
            variables = dict(read_variables(variable_file), **variables)

    # add glonbal exception
    def my_except_hook(exctype, value, traceback):
        publisher.log(f"error executing {args.batch}: {str(value)}")
        printError(f"Global exception: {str(value)}")
        #input('press any key to quit.')
        print("Exception in user code:")
        print('-'*60)
        traceback.print_exc(file=sys.stdout)
        print('-'*60)
        #input('press any key to quit.')
        sys.__excepthook__(exctype, value, traceback)

    sys.excepthook = my_except_hook
    property_bag = PropertyBag(dict(commandParameters,**variables))  
    if args.readonly != None:
        property_bag.readonly = args.readonly
    if args.session != "" :
        property_bag.session_name = args.session
    if args.resource_path != "":
        property_bag.resource_path = args.resource_path
    if args.output != "":
        property_bag.add('output', args.output)
    try:
        config = Config(args.config)
    except Exception as e:
        publisher.errorInfo(property_bag.session_name, f"error in {args.config}: {str(e)}")
        raise

    # Run batch
    if args.batch != None:
        publisher.log(f"info: starting {args.batch}")
        property_bag.user_input = False  # in batch jobs lets start with no user input (one can still enable in the batch file)
        workingDirectory = os.path.dirname(args.batch)
        session = Session(config.apis(), workingDirectory,  property_bag)    
        session.executeBatch(args.batch)
    else:
        session = Session(config.apis(), os.getcwd(),  property_bag)    
        session.start()

if __name__ == '__main__':
    main()