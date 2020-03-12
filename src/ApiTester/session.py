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

class Session:
    def __init__(self, apis, commandParameters):
        self.apis = apis
        self.commandParameters = commandParameters
        self.commandExecutor = Command()

    def start(self):
        quit = False
        self.display()
        while quit == False:
            printPrompt(">>")
            command = input("")
            parser = InputParser()
            parser.parse(command)
            if parser.route == "quit" or parser.route == 'q':
                quit = True
            if parser.route.startswith('help'):
                self.executeHelp(parser)
            else:
                self.executeCommand(parser)

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
                self.execute(foundApiInfo, parser, self.commandParameters)

    def execute(self, apiInfo, parser, commandParameters):
        try:
            data = transform(apiInfo.body, commandParameters)
            path = transformString('path', apiInfo.path, commandParameters)
            baseUrl = transformString('baseUrl', apiInfo.baseUrl, commandParameters)

            apiInfoWithData = ApiInfo(
                apiInfo.api, apiInfo.route, path, baseUrl, data, apiInfo.headers)
            jsonData = ""
            if parser.method.lower() == 'post':
                if len(parser.fileName) == 0:
                    raise Exception('post requires filename')
                with open(parser.fileName, 'r') as in_file:
                    jsonData = json.load(in_file)
            self.commandExecutor.execute(apiInfoWithData, method = parser.method, json = jsonData)
        except ValueError as v:
            printError(str(v))
        except ApiException as ae:
            printError(str(ae))
        except Exception as e:
            printError(str(e))
            print("Exception in user code:")
            print ('-'*60)
            traceback.print_exc(file=sys.stdout)
            print ('-'*60)

    def executeBatch(self, fileName):
        with open(fileName, "r") as file:
            for line in file.readlines():
                command = line.rstrip("\n")
                if len(command) > 0 and command.startswith("#") == False:
                    self.executeCommand(command)

    def executeHelp(self, command):
        self.display()
        #     return
        # routeName = parts[1]
        # pathName = None
        # if len(parts) == 3:
        #     pathName = parts[2]

        # apiInfos = self.apis.get(routeName, None)    
        # if apiInfos == None:
        #     print(f'{routeName} is not valid route')
        #     return
        # if pathName == None:        # help for route
        #     for path, apiInfo in apiInfos.items():
        #         printPath(f"\t\t{path}")
        #     return
        # foundApiInfo = apiInfos.get(pathName, None)
        # if foundApiInfo == None:
        #     print(f'{routeName}.{pathName} is not found')
        #     return
        # print(f"\t    api:{foundApiInfo.api}")
        # print(f"\t  route:{foundApiInfo.route}")
        # print(f"\t   path:{foundApiInfo.path}")
        # print(f"\tbaseUrl:{foundApiInfo.baseUrl}")
        # print(f"\t   body:{foundApiInfo.body}")
        # print(f"\theaders:{foundApiInfo.headers}")
        

if __name__ == "__main__":
    config = Config(sys.argv[1])
    session = Session(config.apis())
    session.start()
