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
from properties import Properties
from inputparser import parseCommand
from transform import transform

class Session:
    def __init__(self, apis, workingDirectory, properties):
        self.apis = apis
        self.workingDirectory = workingDirectory
        self.properties = properties
        self.commandExecutor = Command(properties)

    def start(self):
        quit = False
        self.display()
        while quit == False:
            printPrompt(">>")
            try:
                command = input("")
                if self.executeCommandInput(command) == False:
                    quit = True
            except ValueError as v:
                printError(str(v))
            except ApiException as ae:
                printError(str(ae))
            except Exception as e:
                printError(str(e))
                print("Exception in user code:")
                print('-'*60)
                traceback.print_exc(file=sys.stdout)
                print('-'*60)

    def executeCommandInput(self, command):
        request = parseCommand(command, self.workingDirectory, self.apis, self.properties.properties)
        if request == None:
            return False
        self.commandExecutor.execute(request)
        return True

    def display(self):
        print()
        print('---------------------------')
        print('utility to execute api routes')
        print('---------------------------')
        for name, apiInfos in self.apis.items():
            printRoute(f"\t{name}")
            for path, apiInfo in apiInfos.items():
                printPath(f"\t\t{path}")
        print('-----------------------')
        print('enter route.path format (ex: accesstoken.password) (if u see _ then you can type the route only. accesstoken')
        print('quit/q for quit.')
        print('help for help.')
        print('help.routename for route help.')
        print('help.routename.pathname route path.')
        print('-----------------------')

    def executeBatch(self, fileName):
        with open(fileName, "r") as file:
            for line in file.readlines():
                command = line.rstrip("\n")
                if len(command) > 0 and command.startswith("#") == False:
                    final_command = transform({"command": command}, self.properties.properties)
                    try:
                        self.executeCommandInput(final_command.get('command'))
                    except ApiException as ae:
                        printError(str(ae))

if __name__ == "__main__":
    config = Config(sys.argv[1])
    session = Session(config.apis())
    session.start()
