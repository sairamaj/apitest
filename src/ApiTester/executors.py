# https://riptutorial.com/python/example/23083/why-how-to-use-abcmeta-and--abstractmethod
import json
from oauth import OAuth
from apiresponse import ApiResponse
from pprint import pprint
from api import Api, ApiException
from abc import ABCMeta, abstractstaticmethod
from executorRequest import ApiExecutorRequest, SetExecutorRequest, SetGroupExecutorRequest,  HelpExecutorRequest, \
    ManagementCommandExecutorRequest
from executorRequest import WaitForUserInputExecutorRequest, ExtractVariableExecutorRequest, ConvertJsonToHtmlExecutorRequest
from executorRequest import AssertExecutorRequest, ConvertJsonToHtmlExecutorRequest, JavaScriptExecutorRequest
from executorRequest import AssertsExecutorWithJsRequest, HttpRequestExecutorRequest, FuncCommandExecutorRequest, \
    PrintCommandExecutorRequest
from ui import printRoute, printPath, printInfo, waitForUserInput
from jsonpath_ng.ext import parse
from json2html import *
from transform import getVariables
from utils import readAllText
from commandinfo import getCommandsInfo
from js_executor import JsExecutor
from jpath_extractor import JPathExtractor
from http_request import HttpRequest
from http_request2 import HttpRequest2
from http_requestdata import HttpRequestData
from publish.publisher import Publisher
from functionevaluator import FuncEvaluator


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

    def __init__(self, property_bag):
        self.property_bag = property_bag
        self.publisher = Publisher(property_bag.properties)

    def extractResponseContent(self, response):
        return response.__dict__['_content'].decode("utf-8")

    @abstractstaticmethod
    def execute(executorRequest):
        """The required execute method which all command obejcts will use"""


class AccessTokenExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag
        super(AccessTokenExecutor, self).__init__(property_bag)

    def execute(self, executorRequest):
        if isinstance(executorRequest, ApiExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of ApiExecutorRequest")
        printInfo(f"executing: {executorRequest.apiInfo.baseUrl}")
        oauth = OAuth(executorRequest.apiInfo)
        try:
            response = oauth.getAccessToken()
            self.property_bag.last_http_request = HttpRequest(oauth.response)
            self.publisher.apiresult(
                oauth.response, self.property_bag.session_name)
            pprint(response)
            self.property_bag.access_token = response["access_token"]
        except Exception as e:
            self.publisher.apiresult(
                oauth.response, self.property_bag.session_name)
            self.property_bag.last_http_request = HttpRequest(oauth.response)
            raise


class ApiExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag
        super(ApiExecutor, self).__init__(property_bag)

    def execute(self, executorRequest):
        if isinstance(executorRequest, ApiExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of ApiExecutorRequest")
        printInfo(f"executing: {executorRequest.apiInfo.baseUrl}")
        api = Api(executorRequest.apiInfo, self.property_bag.access_token)
        try:
            if executorRequest.method == 'get':
                response = api.get()
            elif executorRequest.method == 'post':
                response = api.post(executorRequest.payLoad)
            elif executorRequest.method == 'patch':
                response = api.patch(executorRequest.payLoad)
            elif executorRequest.method == 'put':
                response = api.put(executorRequest.payLoad)
            elif executorRequest.method == 'delete':
                response = api.delete(executorRequest.payLoad)
            else:
                raise ValueError(f"{executorRequest.method} not supported.")

            self.property_bag.last_http_request = HttpRequest(api.response)

            self.publisher.apiresult(
                api.response, self.property_bag.session_name)
            pprint(response)
        except ApiException as ae:
            self.property_bag.last_http_request = HttpRequest(api.response)
            self.publisher.apiresult(
                api.response, self.property_bag.session_name)
        except Exception as e:
            self.property_bag.last_http_request = HttpRequest(api.response)
            self.publisher.apiresult(
                api.response, self.property_bag.session_name)
            raise
        finally:
            pass


class SetExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag
        super(SetExecutor, self).__init__(property_bag)

    def execute(self, executorRequest):
        if isinstance(executorRequest, SetExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of SetExecutorRequest")
        if executorRequest.parameterName == "user_input":
            self.property_bag.user_input = executorRequest.parameterValue
        else:
            self.property_bag.properties = dict(self.property_bag.properties,
                                                **{executorRequest.parameterName: executorRequest.parameterValue})


class SetGroupExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag
        super(SetGroupExecutor, self).__init__(property_bag)

    def execute(self, executorRequest):
        if isinstance(executorRequest, SetGroupExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of SetGroupExecutorRequest")

        self.property_bag.properties = dict(
            self.property_bag.properties, **executorRequest.key_value_pairs)


class ListPropertiesExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag
        super(ListPropertiesExecutor, self).__init__(property_bag)

    def execute(self, executorRequest):
        for key, val in self.property_bag.properties.items():
            if type(val) is str:
                print(f"{key.rjust(30)} : {val[:30]}")
            else:
                print(f"{key.rjust(30)} : {val}")
        additional = self.property_bag.additional_properties()
        for key, val in additional.items():
            print(f"{key.rjust(30)} : {str(val)}")


class HelpExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag
        super(HelpExecutor, self).__init__(property_bag)

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
        print('!assert json_path expected_value')
        print('!asserts_with_js assert_file [optional delimiter]')
        print('!extract jsonpath variable (extracts fron last response into variable given json path).')
        print('!extract_from_request jsonpath variable (extracts from last request  into variable given json path).')
        print('!js jsfilename (executes java script file).')
        print('!setgroup variablegroupfilename (to set variables from file).')
        print('!set name=value (to set variable).')
        print('!list (to list all variables).')
        print('!convert_json_html (Converts JSON file to HTML file).')
        print('!management apicommands (gets avarialble api commands ).')
        print('!management commands (gets avarialble commands ).')
        print('!management variables (gets avarialble api variables ).')
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
        super(ManagementCommandExecutor, self).__init__(property_bag)

    def execute(self, executorRequest):
        if isinstance(executorRequest, ManagementCommandExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of ManagementCommandExecutorRequest")
        if executorRequest.request == "commands":
            publisher.managementInfo(self.property_bag.session_name,
                                     "commands", getCommandsInfo())
        elif executorRequest.request == "apicommands":
            commands = {}
            for name, apiInfos in executorRequest.apis.items():
                subcommands = []
                for path, apiInfo in apiInfos.items():
                    subcommands.append(path)
                commands[name] = subcommands
            print(f"apis : {type(executorRequest.apis)}")
            self.publisher.managementInfo(self.property_bag.session_name,
                                          "apicommands", commands)
        elif executorRequest.request == "variables":
            variables = getVariables(readAllText(
                self.property_bag.config_filename))
            self.publisher.managementInfo(self.property_bag.session_name,
                                          "variables", variables)
            print(variables)
        else:
            raise ValueError(
                f"invalid {executorRequest.request} management command (avaiable are commands and variables")


class WaitForUserInputCommandExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag
        super(WaitForUserInputCommandExecutor, self).__init__(property_bag)

    def execute(self, executorRequest):
        if isinstance(executorRequest, WaitForUserInputExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of WaitForUserInputExecutorRequest")
        waitForUserInput(executorRequest.prompt)


class ExtractVariableCommandExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag
        super(ExtractVariableCommandExecutor, self).__init__(property_bag)

    def execute(self, executorRequest):
        variable_path = executorRequest.json_path
        variable_name = executorRequest.variable_name
        from_source = executorRequest.from_source
        variable_value = ""
        try:
            if isinstance(executorRequest, ExtractVariableExecutorRequest) == False:
                raise ValueError(
                    f"{type(executorRequest)} is not of ExtractVariableExecutorRequest")
            if self.property_bag.last_http_request == None:
                raise ValueError(
                    "no last_http_request avialble, please execute any api request to extract variables.")
            if from_source == 'request':
                variable_value = JPathExtractor(json.loads(
                    self.property_bag.last_http_request.request)).extract(variable_path)
            else:
                variable_value = JPathExtractor(json.loads(
                    self.property_bag.last_http_request.response)).extract(variable_path)
        except Exception as e:
            success = False
            message = str(e)

            self.publisher.extractInfo(self.property_bag.session_name,
                                       variable_name, "", False, str(e))
            raise

        self.property_bag.add(variable_name, variable_value)
        self.publisher.extractInfo(self.property_bag.session_name,
                                   variable_name, variable_value, True, "")


class AssertCommandExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag
        super(AssertCommandExecutor, self).__init__(property_bag)

    def execute(self, executorRequest):
        if isinstance(executorRequest, AssertExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of AssertExecutorRequest")

        if self.property_bag.last_http_request == None:
            raise ValueError("No http request yet, please execute any api")

        json_path = executorRequest.json_path
        expected = executorRequest.value
        actual = ""
        try:
            # check whether variable exists in property bag or not
            if json_path == "status_code":  # special case
                actual = self.property_bag.last_http_request.status_code
            else:
                actual = JPathExtractor(json.loads(
                    self.property_bag.last_http_request.response)).extract(json_path)
            if actual == None:
                raise ValueError(
                    f"!assert: variable {executorRequest.variable_name} not found")
            print(f"asserting: {actual} ---> {executorRequest.value}")
            assert str(actual) == str(
                executorRequest.value), f"Did not match. expected:{executorRequest.value} actual:{actual}"
        except AssertionError as e:
            self.publisher.assertInfo(self.property_bag.session_name,
                                      json_path, expected, actual, False, str(e))
            raise

        self.publisher.assertInfo(self.property_bag.session_name,
                                  json_path, expected, actual, True, "success")


class AssertsJsRequestCommandExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag
        super(AssertsJsRequestCommandExecutor, self).__init__(property_bag)

    def execute(self, executorRequest):
        if isinstance(executorRequest, AssertsExecutorWithJsRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of AssertsExecutorWithJsRequest")
        print(f"Executing: {executorRequest.js_file}")
        js = JsExecutor(executorRequest.js_file)
        try:
            script_response = js.execute_postprocess_with_asserts(
                self.property_bag.last_http_request.response, executorRequest.assert_records)
            if script_response == None:
                script_response = "success"
            self.publisher.jsExecuteInfo(self.property_bag.session_name,
                                         executorRequest.js_file, script_response, False)
        except Exception as e:
            self.publisher.jsExecuteInfo(self.property_bag.session_name,
                                         executorRequest.js_file, str(e), True)


class ConvertJsonToHtmlCommandExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag
        super(ConvertJsonToHtmlCommandExecutor, self).__init__(property_bag)

    def execute(self, executorRequest):
        if isinstance(executorRequest, ConvertJsonToHtmlExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of ConvertJsonToHtmlExecutorRequest")
        out_html = json2html.convert(
            json=self.readAllText(executorRequest.json_filename))
        with open(executorRequest.html_filename, 'w') as file:
            file.write(out_html)
        print(f"{executorRequest.html_filename} written successfully.")

    def readAllText(self, fileName):
        with open(fileName, 'r') as file:
            return file.read()


class JavaScirptCommandExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag
        super(JavaScirptCommandExecutor, self).__init__(property_bag)

    def execute(self, executorRequest):
        if isinstance(executorRequest, JavaScriptExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of JavaScriptExecutorRequest")
        print(f"Executing: {executorRequest.js_file}")
        js = JsExecutor(executorRequest.js_file)
        try:
            script_response = js.execute_postprocess(
                'this is request', self.property_bag.last_http_request.response, executorRequest.script_args)
            if script_response == None:
                script_response = "success"
            self.publisher.jsExecuteInfo(self.property_bag.session_name,
                                         executorRequest.js_file, script_response, False)
        except Exception as e:
            self.publisher.jsExecuteInfo(self.property_bag.session_name,
                                         executorRequest.js_file, str(e), True)


class HttpRequestCommandExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag
        super(HttpRequestCommandExecutor, self).__init__(property_bag)

    def execute(self, executorRequest):
        if isinstance(executorRequest, HttpRequestExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of HttpRequestExecutorRequest")
        print(
            f"Executing: {executorRequest.request_file} with requestid:{executorRequest.request_id}")

        try:
            req_data = HttpRequestData(executorRequest.request_file)
            request2 = HttpRequest2(
                req_data.url, req_data.method, req_data.headers, req_data.request)
            response = request2.get()
            self.publisher.apiresult(response.response, self.property_bag.session_name,
                                     executorRequest.request_id)
        except Exception as e:
            self.publisher.apiresult(response, self.property_bag.session_name,
                                     executorRequest.request_id)
            self.property_bag.last_http_request = HttpRequest(response)


class FuncCommandExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag
        super(FuncCommandExecutor, self).__init__(property_bag)

    def execute(self, executorRequest):
        if isinstance(executorRequest, FuncCommandExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of FuncCommandExecutorRequest")
        FuncEvaluator(self.property_bag.last_http_request).evaluate(
            executorRequest.name, executorRequest.args)


class PrintCommandExecutor(ICommand):
    def __init__(self, property_bag):
        self.property_bag = property_bag
        super(PrintCommandExecutor, self).__init__(property_bag)

    def execute(self, executorRequest):
        if isinstance(executorRequest, PrintCommandExecutorRequest) == False:
            raise ValueError(
                f"{type(executorRequest)} is not of PrintCommandExecutorRequest")
        print(f"{executorRequest.message}")
        self.publisher.printInfo(
            self.property_bag.session_name, executorRequest.message)
