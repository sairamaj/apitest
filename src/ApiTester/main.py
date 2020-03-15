import sys
import argparse
from exceptions import ApiException
from pprint import pprint
from transform import transform
from command import Command
from ui import printError
from ui import printPrompt
from session import Session
from config import Config
from properties import Properties

# Load file
parser = argparse.ArgumentParser()
parser.add_argument("--batch", help="Batch file name")
parser.add_argument("--config", help="Config File")
parser.add_argument("--varfile", help="Variables file")
args, unknown = parser.parse_known_args()

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
            parts = line.split('=')
            if len(parts) > 1 :
                variables[parts[0]] = parts[1]

properties = Properties(dict(commandParameters,**variables))  
config = Config(args.config)
session = Session(config.apis(), properties)

# Run batch
if args.batch != None:
    session.executeBatch(args.batch)
else:
    session.start()
