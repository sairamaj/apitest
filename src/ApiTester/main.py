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
from properties import Properties
from pipeserver import PipeServer

# Load file
parser = argparse.ArgumentParser()
parser.add_argument("--batch", help="Batch file name")
parser.add_argument("--config", help="Config File")
parser.add_argument("--varfile", help="Variables file")
parser.add_argument("--session", help="Session name")
args, unknown = parser.parse_known_args()

loggerPipe = PipeServer('log')

if args.config == None:
    printError('Config file is required . [ex: main.py --config apigee.json] ')
    parser.print_help()
    exit(-1)

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
                variables[parts[0]] = parts[1]

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
properties = Properties(dict(commandParameters,**variables))  
if args.session != "" :
    properties.session_name = args.session
config = Config(args.config)

# Run batch
if args.batch != None:
    loggerPipe.send(f"info: starting {args.batch}")
    try:
        workingDirectory = os.path.dirname(args.batch)
        session = Session(config.apis(), workingDirectory,  properties)    
        session.executeBatch(args.batch)
    finally:
        loggerPipe.close()
else:
    session = Session(config.apis(), os.getcwd(),  properties)    
    session.start()
