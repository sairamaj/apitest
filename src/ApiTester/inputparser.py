import os
import sys
import json
import copy
from apiinfo import ApiInfo
from abc import ABCMeta, abstractstaticmethod
from executorRequest import ApiExecutorRequest, SetExecutorRequest, ListExecutorRequest, HelpExecutorRequest
from executorRequest import ManagementCommandExecutorRequest, WaitForUserInputExecutorRequest, ExtractVariableExecutorRequest
from executorRequest import AssertExecutorRequest, ConvertJsonToHtmlExecutorRequest, JavaScriptExecutorRequest
from executorRequest import AssertsExecutorWithJsRequest, HttpRequestExecutorRequest
from transform import transform
from transform import transformString, transformValue
from utils import readAllText, read_key_value_pairs, line_parser
from resource_provider import ResourceProvider


def parseCommand(command, workingDirectory, apis, property_bag):
    commands = {
        '!assert': AssertRequestInputParser(workingDirectory),
        '!asserts_with_js': AssertsWithJsRequestInputParser(workingDirectory),
        '!extract': ExtractVariableRequestInputParser(workingDirectory),
        '!httprequest': HttpRequestInputParser(workingDirectory),
        '!js': JavaScriptRequestInputParser(workingDirectory),
        '!list': ListCommandInputParser(workingDirectory),
        '!set': SetCommandInputParser(workingDirectory),
        '!help': HelpCommandInputParser(workingDirectory),
        '!management': ManagementCommandRequestInputParser(workingDirectory),
        '!convert_json_html': ConvertJsonToHtmlRequestInputParser(workingDirectory),
        '!waitforuserinput': WaitForUserInputRequestInputParser(workingDirectory),
    }

    parts = command.split(' ')
    commandName = parts[0].lower()
    parser = None
    if commandName == 'quit' or commandName == 'q':
        return None
    parser = commands.get(commandName, None)
    if parser == None:
        parser = ApiCommandInputParser(workingDirectory)

    return parser.parseCommand(command, apis, property_bag)


class InputParser:
    def __init__(self):
        self.command = ''

    @abstractstaticmethod
    def parseCommand(command):
        """The required parse method which all command obejcts will use"""


class ApiCommandInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, property_bag):
        parts = command.split(' ')
        # ex:
        #   users
        #   users post file
        method = 'get'
        filename = ''
        route = ''
        path = ''
        if len(parts) > 1:
            method = parts[1].lower()
            if len(parts) > 2:
                filename = parts[2]
        parts = parts[0].split('.')
        if len(parts) > 1:
            route = parts[0]
            path = parts[1]
        else:
            route = parts[0]
            path = "_"

        supportedVerbs = ['get', 'post', 'patch']
        if method in ['get', 'post', 'patch'] == False:
            raise ValueError(
                f"{method} not supported, supported are {supportedVerbs}")
        apiInfos = apis.get(route, None)

        if apiInfos == None:
            raise ValueError(f"{route} not found")
        else:
            foundApiInfo = apiInfos.get(path, None)
            if foundApiInfo == None:
                raise ValueError(f"{route}.{path} not found")
            foundApiInfo = copy.deepcopy(
                foundApiInfo)      # we need fresh copy

        data = transform(foundApiInfo.body, property_bag.properties)
        path = transformString('path', foundApiInfo.path,
                               property_bag.properties)
        baseUrl = transformString(
            'baseurl', foundApiInfo.baseUrl, property_bag.properties)

        transformedHeaders = transform(
            foundApiInfo.headers, property_bag.properties)

        apiInfoWithData = ApiInfo(
            foundApiInfo.api, foundApiInfo.route, path, baseUrl, data, transformedHeaders)
        jsonData = ""
        if method == 'post' or method == 'patch':
            if len(filename) == 0:
                raise Exception(f'{method} requires filename')
            fileNameWithPath = ResourceProvider(
                property_bag.resource_path).api_filepath_for_http_verb(filename, method)
            post_data = json.loads(readAllText(fileNameWithPath))
            tranform_items = {"temp": post_data}
            tranformed_items = transform(
                tranform_items, property_bag.properties)
            jsonData = tranformed_items["temp"]
            print(f"--------> {jsonData}")
            print(f"--------> {type(jsonData)}")

        return ApiExecutorRequest(apiInfoWithData, jsonData, method)

    def __str__(self):
        return "todo"


class SetCommandInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, property_bag):
        args = line_parser(command)
        # ex:
        #   set param=value
        if len(args) < 2:
            raise ValueError("set require name=value format")
        name = args[1]
        value = ""
        if len(args) > 2:
            value = args[2]
        # go through transformation
        value = transformValue(value, property_bag.properties)

        return SetExecutorRequest(name, value)


class ListCommandInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, property_bag):
        return ListExecutorRequest()


class HelpCommandInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, property_bag):
        return HelpExecutorRequest(apis)


class ManagementCommandRequestInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, property_bag):
        parts = command.split(' ')
        if len(parts) < 2:
            raise ValueError(
                f"!management command requires requestName (ex: !management commands)")
        # check supported management commands
        supportedMgmtCommands = ['commands', 'apicommands', 'variables']
        mgmtRequest = parts[1]
        if mgmtRequest in supportedMgmtCommands:
            return ManagementCommandExecutorRequest(apis, mgmtRequest)
        raise ValueError(
            f"!management {mgmtRequest} is not available, supported requests for management are: {supportedMgmtCommands}")


class WaitForUserInputRequestInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, property_bag):
        parts = command.split(' ')
        prompt = ""
        if len(parts) > 1:
            prompt = parts[1]
        prompt += "(press any key to):"
        return WaitForUserInputExecutorRequest(prompt)


class ExtractVariableRequestInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, property_bag):
        parts = command.split(' ')
        if len(parts) < 3:
            raise ValueError(
                "!extract requires jsonPath and variable name ( ex !extract user.name userName)")
        return ExtractVariableExecutorRequest(parts[1], parts[2])


class AssertRequestInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, property_bag):
        parts = command.split(' ')
        if len(parts) < 3:
            raise ValueError(
                "!assert requires json_path and value_to_match ( ex !assert json_path value_to_match)")
        return AssertExecutorRequest(parts[1], parts[2])


class AssertsWithJsRequestInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, property_bag):
        parts = command.split(' ')
        if len(parts) < 2:
            raise ValueError(
                "!asserts_with_js assertfile [optional delimiter]( ex !asserts_with_js input.asserts |)")
        assert_file = ResourceProvider(
            property_bag.resource_path).asserts_filepath(parts[1])
        if os.path.exists(assert_file) == False:
            raise ValueError(f"{assert_file} does not exists.")

        delimiter = '|'
        if len(parts) > 2:
            delimiter = parts[2]
        assert_records = transform(read_key_value_pairs(
            assert_file, delimiter), property_bag.properties)

        js_file = ResourceProvider(
            property_bag.resource_path).js_filepath('asserts_json.js')
        return AssertsExecutorWithJsRequest(js_file, assert_records)


class ConvertJsonToHtmlRequestInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, property_bag):
        parts = command.split(' ')
        if len(parts) < 3:
            raise ValueError(
                "!convert_json_html requires inputfile and outputfile ( ex !convert_json_html inputJsonFileName outputJsonFileName)")
        return ConvertJsonToHtmlExecutorRequest(parts[1], parts[2])


class JavaScriptRequestInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, property_bag):
        parts = command.split(' ')
        if len(parts) < 2:
            raise ValueError(
                "!js requires javascript file name (ex: !js validateuser.js) ")
        js_file = ResourceProvider(
            property_bag.resource_path).js_filepath(parts[1])
        if os.path.exists(js_file) == False:
            raise ValueError(f"{js_file} does not exists.")
        script_args = {}
        for tag_value_pair in parts[2:]:
            arg_parts = tag_value_pair.split('=')
            if len(arg_parts) > 1:
                script_args[arg_parts[0]] = arg_parts[1]
        return JavaScriptExecutorRequest(js_file, script_args)


class HttpRequestInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, property_bag):
        parts = command.split(' ')
        if len(parts) < 2:
            raise ValueError(
                "!httprequest requires request_filename [optional request_id] (ex: !httprequest sample.json [request_id]) ")
        request_file = parts[1]
        request_id = ""
        if len(parts) > 2:
            request_id = parts[2]

        if os.path.exists(request_file) == False:
            raise ValueError(f"{request_file} does not exists.")

        return HttpRequestExecutorRequest(request_file, request_id)
