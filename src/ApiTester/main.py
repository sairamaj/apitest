import os
import sys
import argparse
import traceback
from exceptions import ApiException
from pprint import pprint
from transform import transform
from command import Command
from ui import printError
from ui import printPrompt
from session import Session
from config import Config
from property_bag import PropertyBag
from pipeserver import PipeServer
from transform import transformValue

def main():
    # Load file
    parser = argparse.ArgumentParser()
    parser.add_argument("--batch", help="Batch file name")
    parser.add_argument("--config", help="Config File")
    parser.add_argument("--varfile", help="Variables file")
    parser.add_argument("--session", help="Session name")
    parser.add_argument("--resource_path", help="Resource Path")
    args, unknown = parser.parse_known_args()

    loggerPipe = PipeServer('log')

    if args.config == None:
        printError('Config file is required . [ex: main.py --config apigee.json] ')
        parser.print_help()
        sys.exit(-1)

    # convert unknowns
    items = {x.split('=')[0][2:]: x.split('=')[-1] for x in unknown if x[:2] == '--'}
    commandParameters = dict(args.__dict__, **items)
        
    # Load variables
    variables = {}
    if args.varfile != None:
        with open(args.varfile, 'r') as in_file:
            for line in in_file.readlines():
                parts = line.strip().split('=')
                if len(parts) > 1 :
                    if parts[0].startswith('#') == False:
                        variables[parts[0]] =transformValue(parts[1], variables)

    # add glonbal exception
    def my_except_hook(exctype, value, traceback):
        loggerPipe.send(f"error executing {args.batch}: {str(value)}")
        printError(f"Global exception: {str(value)}")
        input('press any key to quit.')
        loggerPipe.close()
        print("Exception in user code:")
        print('-'*60)
        traceback.print_exc(file=sys.stdout)
        print('-'*60)
        input('press any key to quit.')
        sys.__excepthook__(exctype, value, traceback)

    sys.excepthook = my_except_hook
    property_bag = PropertyBag(dict(commandParameters,**variables))  
    if args.session != "" :
        property_bag.session_name = args.session
    if args.resource_path != "":
        property_bag.resource_path = args.resource_path
    config = Config(args.config)

    # Run batch
    if args.batch != None:
        loggerPipe.send(f"info: starting {args.batch}")
        try:
            workingDirectory = os.path.dirname(args.batch)
            session = Session(config.apis(), workingDirectory,  property_bag)    
            session.executeBatch(args.batch)
        finally:
            loggerPipe.close()
    else:
        session = Session(config.apis(), os.getcwd(),  property_bag)    
        session.start()

if __name__ == '__main__':
    main()