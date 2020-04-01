import sys
import traceback
from config import Config
from ui import printPrompt
from ui import printRoute
from ui import printPath
from command import Command
from apiinfo import ApiInfo
from ui import printError
from exceptions import ApiException
from inputparser import InputParser
from property_bag import PropertyBag
from inputparser import parseCommand
from transform import transform
from logcollector import sendErrorInfo


class Session:
    def __init__(self, apis, workingDirectory, property_bag):
        self.apis = apis
        self.workingDirectory = workingDirectory
        self.property_bag = property_bag
        self.commandExecutor = Command(property_bag)

    def start(self):
        quit = False
        self.executeCommandInput("!help")   # show help to start with
        while quit == False:
            printPrompt(">>")
            command = input("")
            if self.executeCommandInput(command) == False:
                quit = True

    def executeCommandInput(self, command):
        error = None
        try:
            request = parseCommand(
                command, self.workingDirectory, self.apis, self.property_bag.properties)
            if request == None:
                return False
            self.commandExecutor.execute(request)
            return True
        except AssertionError as v:
            error = v
            printError(str(v))
        except ValueError as v:
            error = v
            printError(str(v))
        except ApiException as ae:
            # don't set error info for Api exception as notificaiton it is already taken care
            printError(str(ae))
        except Exception as e:
            error = e
            printError(str(e))
            print("Exception in user code:")
            print('-'*60)
            traceback.print_exc(file=sys.stdout)
            print('-'*60)

        if error != None:
            sendErrorInfo(self.property_bag.session_name, error)

    def executeBatch(self, fileName):
        with open(fileName, "r") as file:
            for line in file.readlines():
                command = line.rstrip("\n")
                if len(command) > 0 and command.startswith("#") == False:
                    if command == "!return":
                        return  # stop the batch commands
                    final_command = transform(
                        {"command": command}, self.property_bag.properties)
                    try:
                        self.executeCommandInput(final_command.get('command'))
                    except ApiException as ae:
                        printError(str(ae))
