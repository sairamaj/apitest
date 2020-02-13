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
parser.add_argument("--claims", help="Claims temporary")
args = parser.parse_args()

if args.config == None:
    printError('Config file is required . [ex: main.py --config apigee.json] ')
    parser.print_help()
    exit(-1)

config = config.Config(args.config)

cmd = Command()

def executeCommand(command):
    apiConfig = config.get(command)
    data = apiConfig.get("body", None)
    if data != None:
        data = transform(data, args.__dict__)
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

        if command == "quit":
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

