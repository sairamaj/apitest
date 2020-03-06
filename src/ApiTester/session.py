import sys
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
            if command == "quit" or command == 'q':
                quit = True
            if command.startswith('help'):
                self.executeHelp(command)
            else:
                self.executeCommand(command)

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

    def parseCommand(self, command):
        parts = command.split('.')
        if len(parts) > 1:
            return parts[0], parts[1]
        return parts[0], "_"

    def executeCommand(self, command):
        route, path = self.parseCommand(command)
        apiInfos = self.apis.get(route, None)
        if apiInfos == None:
            print(f"{route} not found")
        else:
            foundApiInfo = apiInfos.get(path, None)
            if foundApiInfo == None:
                print('not found')
            else:
                self.execute(foundApiInfo, self.commandParameters)

    def execute(self, apiInfo, commandParameters):
        try:
            data = transform(apiInfo.body, commandParameters)
            path = transformString('path', apiInfo.path, commandParameters)

            apiInfoWithData = ApiInfo(
                apiInfo.api, apiInfo.route, path, apiInfo.baseUrl, data, apiInfo.headers)
            self.commandExecutor.execute(apiInfoWithData)
        except ValueError as v:
            printError(str(v))
        except ApiException as ae:
            printError(str(ae))

    def executeBatch(self, fileName):
        with open(fileName, "r") as file:
            for line in file.readlines():
                command = line.rstrip("\n")
                if len(command) > 0 and command.startswith("#") == False:
                    self.executeCommand(command)

    def executeHelp(self, command):
        parts = command.split('.')
        if len(parts) == 1:
            self.display()
            return
        routeName = parts[1]
        pathName = None
        if len(parts) == 3:
            pathName = parts[2]

        apiInfos = self.apis.get(routeName, None)    
        if apiInfos == None:
            print(f'{routeName} is not valid route')
            return
        if pathName == None:        # help for route
            for path, apiInfo in apiInfos.items():
                printPath(f"\t\t{path}")
            return
        foundApiInfo = apiInfos.get(pathName, None)
        if foundApiInfo == None:
            print(f'{routeName}.{pathName} is not found')
            return
        print(f"\t    api:{foundApiInfo.api}")
        print(f"\t  route:{foundApiInfo.route}")
        print(f"\t   path:{foundApiInfo.path}")
        print(f"\tbaseUrl:{foundApiInfo.baseUrl}")
        print(f"\t   body:{foundApiInfo.body}")
        print(f"\theaders:{foundApiInfo.headers}")
        

if __name__ == "__main__":
    config = Config(sys.argv[1])
    session = Session(config.apis())
    session.start()
