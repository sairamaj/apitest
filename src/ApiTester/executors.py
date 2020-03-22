# https://riptutorial.com/python/example/23083/why-how-to-use-abcmeta-and--abstractmethod
import json
from oauth import OAuth
from apiresponse import ApiResponse
from pprint import pprint
from api import Api
from logcollector import collectlog
from abc import ABCMeta, abstractstaticmethod
from executorRequest import ApiExecutorRequest, SetExecutorRequest, HelpExecutorRequest, PipeExecutorRequest
from ui import printRoute
from ui import printPath


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

    @abstractstaticmethod
    def execute(executorRequest):
        """The required execute method which all command obejcts will use"""


class AccessTokenExecutor(ICommand):
    def __init__(self, properties):
        self.properties = properties

    def execute(self, executorRequest):
        if isinstance(executorRequest, ApiExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of ApiExecutorRequest")
        print(f"accesstoken: {executorRequest.apiInfo.baseUrl}")
        oauth = OAuth(executorRequest.apiInfo)
        try:
            response = oauth.getAccessToken()
            collectlog(oauth.response, self.properties.session_name)
            pprint(response)
            self.properties.access_token = response["access_token"]
        except Exception as e:
            collectlog(oauth.response, self.properties.session_name)
            raise


class ApiExecutor(ICommand):
    def __init__(self, properties):
        self.properties = properties

    def execute(self, executorRequest):
        if isinstance(executorRequest, ApiExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of ApiExecutorRequest")

        api = Api(executorRequest.apiInfo, self.properties.access_token)
        try:
            if executorRequest.method == 'get':
                response = api.get()
            else:
                response = api.post(executorRequest.payLoad)
            collectlog(api.response, self.properties.session_name)
            pprint(response)
        except Exception as e:
            collectlog(api.response, self.properties.session_name)
            raise
        finally:
            pass


class SetExecutor(ICommand):
    def __init__(self, properties):
        self.properties = properties

    def execute(self, executorRequest):
        if isinstance(executorRequest, SetExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of SetExecutorRequest")
        self.properties.properties = dict(
            {executorRequest.parameterName: executorRequest.parameterValue}, **self.properties.properties)


class ListPropertiesExecutor(ICommand):
    def __init__(self, properties):
        self.properties = properties

    def execute(self, executorRequest):
        for key, val in self.properties.properties.items():
            print(f"{key.rjust(30)} : {val}")


class HelpExecutor(ICommand):
    def __init__(self, properties):
        self.propeties = properties

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
        print('help for help.')
        print('help.routename for route help.')
        print('help.routename.pathname route path.')
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


class PipeCommandExecutor(ICommand):
    def __init__(self, properties):
        self.properties = properties

    def execute(self, executorRequest):
        if isinstance(executorRequest, PipeExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of PipeExecutorRequest")
        print(f'pipe {executorRequest.request} will be executed here.')