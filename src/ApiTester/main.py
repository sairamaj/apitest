import sys
import config
from exceptions import ApiException
from pprint import pprint
from transform import transform
from command import Command
from ui import printError
from ui import printPrompt
import argparse


# Load file
parser = argparse.ArgumentParser()
parser.add_argument("--batch", help="Batch file name")
parser.add_argument("--config", help="Config File")
parser.add_argument("--client_id", help="Client id")
parser.add_argument("--client_secret", help="Client secret")
args, unknown = parser.parse_known_args()

if args.config == None:
    printError('Config file is required . [ex: main.py --config apigee.json] ')
    parser.print_help()
    exit(-1)

config = config.Config(args.config)


# convert unknowns
items = {x.split('=')[0][2:]: x.split('=')[-1] for x in unknown if x[:2] == '--'}
commandParameters = dict(args.__dict__, **items)
cmd = Command()

def executeCommand(command):
    apiConfig = config.get(command)
    data = apiConfig.get("body", None)
    print('data ', data)
    if data != None:
        print('going to transform')
        data = transform(data, commandParameters)
    print('url ->', apiConfig["url"])
    cmd.execute(command, apiConfig["url"], data)

def runBatch(fileName):
    print(f"executing {fileName}")
    with open(fileName, "r") as file:
        for line in file.readlines():
            command = line.rstrip("\n")
            executeCommand(command)

def runInteractive():
    # Go interactive mode
    quit = False
    while quit == False:
        pprint(config.apis())
        printPrompt(">>")
        command = input("")

        if command == "quit" or command =='q':
            quit = True
        else:
            try:
                executeCommand(command)
            except ValueError as v:
                printError(str(v))
            except ApiException as ae:
                printError(str(ae))

# Run batch
if args.batch != None:
    runBatch(args.batch)
else:
    runInteractive()

