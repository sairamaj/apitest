import os
import sys
import traceback
import json
from transform import transform
from transform import transformString
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
from inputparser import parseCommand, SetCommandInputParser
from executors import ExecutorRequest

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
        parser = parseCommand(command)
        if parser == None:
            return False
        if isinstance(parser, SetCommandInputParser):
            self.commandExecutor.execute(ExecutorRequest(
                parser.command, None, None, None, parser.name, parser.value))
        else:
            self.executeCommand(parser)
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

    def executeCommand(self, parser):
        apiInfos = self.apis.get(parser.route, None)
        if apiInfos == None:
            print(f"{parser.route} not found")
        else:
            foundApiInfo = apiInfos.get(parser.path, None)
            if foundApiInfo == None:
                print('not found')
            else:
                self.execute(foundApiInfo, parser, self.properties.properties)

    def execute(self, apiInfo, parser, propertyDictionary):
        data = transform(apiInfo.body, propertyDictionary)
        path = transformString('path', apiInfo.path, propertyDictionary)
        baseUrl = transformString('baseurl', apiInfo.baseUrl, propertyDictionary)
        
        transformedHeaders = transform(apiInfo.headers,propertyDictionary)

        apiInfoWithData = ApiInfo(
            apiInfo.api, apiInfo.route, path, baseUrl, data, apiInfo.headers)
        jsonData = ""
        if parser.method.lower() == 'post':
            if len(parser.filename) == 0:
                raise Exception('post requires filename')
            fileNameWithPath = os.path.join(self.workingDirectory,parser.filename)
            with open(fileNameWithPath, 'r') as in_file:
                jsonData = json.load(in_file)
        executorRequest = ExecutorRequest(
            parser.command, apiInfoWithData, jsonData, parser.method)
        self.commandExecutor.execute(executorRequest)

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
