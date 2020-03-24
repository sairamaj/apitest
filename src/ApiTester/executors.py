# https://riptutorial.com/python/example/23083/why-how-to-use-abcmeta-and--abstractmethod
import json
from oauth import OAuth
from apiresponse import ApiResponse
from pprint import pprint
from api import Api
from logcollector import collectlog
from abc import ABCMeta, abstractstaticmethod
from executorRequest import ApiExecutorRequest, SetExecutorRequest, HelpExecutorRequest, ManagementCommandExecutorRequest
from executorRequest import WaitForUserInputExecutorRequest, ExtractVariableExecutorRequest
from ui import printRoute, printPath, waitForUserInput
from pipeserver import PipeServer
from jsonpath_ng.ext import parse

managementPipe = PipeServer('management')

class ExecutorRequest:
    def __init__(self, command, apiInfo, payLoad, method, parameterName=None, parameterValue=None):
        self.command = command
        self.apiInfo = apiInfo
        self.payLoad = payLoad
        self.method = method
        self.parameterName = parameterName
        self.parameterValue = parameterValue


class ICommand(metaclass=ABCMeta):
    """The command interface, which all commands will implement"""

    def extractResponseContent(self,response):
        return response.__dict__['_content'].decode("utf-8")

    @abstractstaticmethod
    def execute(executorRequest):
        """The required execute method which all command obejcts will use"""


class AccessTokenExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag

    def execute(self, executorRequest):
        if isinstance(executorRequest, ApiExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of ApiExecutorRequest")
        print(f"accesstoken: {executorRequest.apiInfo.baseUrl}")
        oauth = OAuth(executorRequest.apiInfo)
        try:
            response = oauth.getAccessToken()
            self.property_bag.last_response = self.extractResponseContent(oauth.response)
            collectlog(oauth.response, self.property_bag.session_name)
            pprint(response)
            self.property_bag.access_token = response["access_token"]
        except Exception as e:
            collectlog(oauth.response, self.property_bag.session_name)
            raise


class ApiExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag

    def execute(self, executorRequest):
        if isinstance(executorRequest, ApiExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of ApiExecutorRequest")

        api = Api(executorRequest.apiInfo, self.property_bag.access_token)
        try:
            if executorRequest.method == 'get':
                response = api.get()
            else:
                response = api.post(executorRequest.payLoad)
            collectlog(api.response, self.property_bag.session_name)
            pprint(response)
        except Exception as e:
            collectlog(api.response, self.property_bag.session_name)
            raise
        finally:
            pass


class SetExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag

    def execute(self, executorRequest):
        if isinstance(executorRequest, SetExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of SetExecutorRequest")
        self.property_bag.properties = dict(
            {executorRequest.parameterName: executorRequest.parameterValue}, **self.property_bag.properties)


class ListPropertiesExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag

    def execute(self, executorRequest):
        for key, val in self.property_bag.properties.items():
            if type(val) is str:
                print(f"{key.rjust(30)} : {val[:30]}")
            else:
                print(f"{key.rjust(30)} : {val}")
        additional = {}
        additional['last_response'] = self.property_bag.last_response[:30]
        additional['session'] = self.property_bag.session_name
        for key, val in additional.items():
            print(f"{key.rjust(30)} : {str(val)}")

class HelpExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag

    def display(self, apis):
        print()
        print('---------------------------')
        print('utility to execute api routes')
        print('---------------------------')
        for name, apiInfos in apis.items():
            printRoute(f"\t{name}")
            for path, apiInfo in apiInfos.items():
                printPath(f"\t\t{path}")
        print('-----------------------')
        print('enter route.path format (ex: accesstoken.password) (if u see _ then you can type the route only. accesstoken')
        print('quit/q for quit.')
        print('!help for help.')
        print('!help.routename for route help.')
        print('!help.routename.pathname route path.')
        print('!set name=value (to set variable).')
        print('!list (to list all variables).')
        print('!extract jsonpath variable (extracts data into variable given json path).')
        print('!waitforuserinput <optionalprompt>  (useful in batch jobs to wait before proceeding).')
        print('-----------------------')

    def execute(self, executorRequest):
        if isinstance(executorRequest, HelpExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of HelpExecutorRequest")

        self.display(executorRequest.apis)
        # return
        # routeName = parser.route
        # pathName = parser.path

        # if len(routeName) == 0:
        #     return

        # apiInfos = self.apis.get(routeName, None)
        # if apiInfos == None:
        #     print(f'{routeName} is not valid route')
        #     return

        # if len(pathName) == 0:        # help for route
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


class ManagementCommandExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag

    def execute(self, executorRequest):
        if isinstance(executorRequest, ManagementCommandExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of ManagementCommandExecutorRequest")
        commands = []
        for name, apiInfos in executorRequest.apis.items():
            for path, apiInfo in apiInfos.items():
                commands.append(f"{name}.{path}")
        print(f"apis : {type(executorRequest.apis)}")
        data = json.dumps(commands)
        print(f"{data}")
        managementPipe.send(data)

class WaitForUserInputCommandExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag

    def execute(self, executorRequest):
        if isinstance(executorRequest, WaitForUserInputExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of WaitForUserInputExecutorRequest")
        waitForUserInput(executorRequest.prompt)

class ExtractVariableCommandExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag

    def execute(self, executorRequest):
        if isinstance(executorRequest, ExtractVariableExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of ExtractVariableExecutorRequest")
        if self.property_bag.last_response == None:
            raise ValueError("no last_response avialble, please execute any api request to extract variables.")
        jsonpath_expr = parse(executorRequest.json_path)
        matches = jsonpath_expr.find(json.loads(self.property_bag.last_response))
        print(f"extracting {executorRequest.json_path} to {executorRequest.variable_name}")
        if len(matches) == 0 :
            raise ValueError(f"no match found for {executorRequest.json_path}")
        for match in matches:
            print(match.value)
            print(len(match.value))
            print(type(match.value))
            if type(match.value) is str:
                self.property_bag.add(executorRequest.variable_name,match.value)
            if type(match.value) is list:
                if len(match.value) >1: 
                    raise ValueError(f"found multiple items found for{executorRequest.json_path} count: {len(match.value)}")
                elif len(match.value) == 0: 
                    raise ValueError(f"no value found for {executorRequest.json_path}")
                else:
                    self.property_bag.add(executorRequest.variable_name,match.value[0])

            

                


