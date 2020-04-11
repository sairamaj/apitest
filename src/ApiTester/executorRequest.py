from abc import ABCMeta, abstractstaticmethod
from utils import readAllText

class ExecutorRequest(metaclass=ABCMeta):
    pass


class ApiExecutorRequest(ExecutorRequest):
    def __init__(self, apiInfo, payLoad, method):
        self.command = "api"
        self.apiInfo = apiInfo
        self.payLoad = payLoad
        self.method = method


class SetExecutorRequest(ExecutorRequest):
    def __init__(self, parameterName, parameterValue):
        self.command = "!set"
        self.parameterName = parameterName
        if parameterValue.startswith('__file__:'):
            filename = parameterValue[len('__file__:'):]
            self.parameterValue = readAllText(filename)
        else:
            self.parameterValue = parameterValue


class ListExecutorRequest(ExecutorRequest):
    def __init__(self):
        self.command = "!list"


class HelpExecutorRequest(ExecutorRequest):
    def __init__(self, apis):
        self.command = "!help"
        self.apis = apis


class ManagementCommandExecutorRequest(ExecutorRequest):
    def __init__(self, apis, request):
        self.command = "!management"
        self.request = request
        self.apis = apis

class WaitForUserInputExecutorRequest(ExecutorRequest):
    def __init__(self, prompt):
        self.command = "!waitforuserinput"
        self.prompt = prompt

class ExtractVariableExecutorRequest(ExecutorRequest):
    def __init__(self, json_path, variable_name):
        self.command = "!extract"
        self.json_path = json_path
        self.variable_name = variable_name

class AssertExecutorRequest(ExecutorRequest):
    def __init__(self, json_path, value):
        self.command = "!assert"
        self.json_path = json_path
        self.value = value

class AssertsExecutorWithJsRequest(ExecutorRequest):
    def __init__(self, js_file, assert_records):
        self.command = "!asserts_with_js"
        self.js_file = js_file
        self.assert_records = assert_records

class ConvertJsonToHtmlExecutorRequest(ExecutorRequest):
    def __init__(self, json_filename, html_filename):
        self.command = "!convert_json_html"
        self.json_filename = json_filename
        self.html_filename = html_filename

class JavaScriptExecutorRequest(ExecutorRequest):
    def __init__(self, js_file, script_args):
        self.command = "!js"
        self.js_file = js_file
        self.script_args = script_args

class HttpRequestExecutorRequest(ExecutorRequest):
    def __init__(self, request_file, request_id):
        self.command = "!httprequest"
        self.request_file = request_file
        self.request_id = request_id
    