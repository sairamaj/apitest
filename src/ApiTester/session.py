import sys
from transform import transform
from transform import transformString
from config import Config
from ui import printPrompt
from command import Command
from apiinfo import ApiInfo
from ui import printError
from exceptions import ApiException


class Session:
    def __init__(self, apis, commandParameters):
        self.apis = apis
        self.commandParameters = commandParameters
        self.commandExecutor = Command()
        print(type(self.apis))

    def start(self):
        quit = False
        self.display()
        while quit == False:
            printPrompt(">>")
            command = input("")
            if command == "quit" or command == 'q':
                quit = True
            else:
                print('execute:', command)
                self.executeCommand(command)

    def display(self):
        for name, apiInfos in self.apis.items():
            print(name)
            for path, apiInfo in apiInfos.items():
                print(f"\t{path}")

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
            print(apiInfos)
            foundApiInfo = apiInfos.get(path, None)
            if foundApiInfo == None:
                print('not found')
            else:
                print(
                    f"--->found:{type(foundApiInfo)}: url:{foundApiInfo.baseUrl}:  path:{foundApiInfo.path}")
                self.execute(foundApiInfo, self.commandParameters)

    def execute(self, apiInfo, commandParameters):
        try:
            data = transform(apiInfo.body, commandParameters)
            path = transformString('path', apiInfo.path, commandParameters)
            #cmd.execute(command, apiConfig['url'], data, apiConfig.get('headers',None))
            print(f"executing: {apiInfo.route}")
            print(f"path: {apiInfo.path}")
            print(f"url: {apiInfo.baseUrl}")
            print(f"body: {apiInfo.body}")
            print(f"headers: {apiInfo.headers}")

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


if __name__ == "__main__":
    config = Config(sys.argv[1])
    session = Session(config.apis())
    session.start()
