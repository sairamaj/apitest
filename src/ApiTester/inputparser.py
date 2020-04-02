import os
import sys
import json
import copy
from apiinfo import ApiInfo
from abc import ABCMeta, abstractstaticmethod
from executorRequest import ApiExecutorRequest, SetExecutorRequest, ListExecutorRequest, HelpExecutorRequest
from executorRequest import ManagementCommandExecutorRequest, WaitForUserInputExecutorRequest, ExtractVariableExecutorRequest
from executorRequest import AssertExecutorRequest, ConvertJsonToHtmlExecutorRequest
from transform import transform
from transform import transformString, transformValue
from utils import readAllText

def parseCommand(command, workingDirectory, apis, propertyDictionary):
    commands = {
        '!assert': AssertRequestInputParser(workingDirectory),
        '!extract': ExtractVariableRequestInputParser(workingDirectory),
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

    return parser.parseCommand(command, apis, propertyDictionary)


class InputParser:
    def __init__(self):
        self.command = ''

    @abstractstaticmethod
    def parseCommand(command):
        """The required parse method which all command obejcts will use"""


class ApiCommandInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, propertyDictionary):
        parts = command.split(' ')
        # ex:
        #   users
        #   users post file
        method = 'get'
        filename = ''
        route = ''
        path = ''
        if len(parts) > 1:
            method = parts[1]
            if len(parts) > 2:
                filename = parts[2]
        parts = parts[0].split('.')
        if len(parts) > 1:
            route = parts[0]
            path = parts[1]
        else:
            route = parts[0]
            path = "_"
        
        supportedVerbs = ['get','post','patch']
        if method.tolower() in ['get','post','patch'] == False:
            raise ValueError(f"{method} not supported, supported are {supportedVerbs}")
        apiInfos = apis.get(route, None)

        if apiInfos == None:
            raise ValueError(f"{route} not found")
        else:
            foundApiInfo = apiInfos.get(path, None)
            if foundApiInfo == None:
                raise ValueError(f"{route}.{path} not found")
            foundApiInfo = copy.deepcopy(foundApiInfo)      # we need fresh copy

        data = transform(foundApiInfo.body, propertyDictionary)
        path = transformString('path', foundApiInfo.path, propertyDictionary)
        baseUrl = transformString(
            'baseurl', foundApiInfo.baseUrl, propertyDictionary)

        transformedHeaders = transform(
            foundApiInfo.headers, propertyDictionary)

        apiInfoWithData = ApiInfo(
            foundApiInfo.api, foundApiInfo.route, path, baseUrl, data, transformedHeaders)
        jsonData = ""
        if method.lower() == 'post' or method.lower() == 'patch':
            if len(filename) == 0:
                raise Exception(f'{method.lower()} requires filename')
            fileNameWithPath = os.path.join(self.workingDirectory, filename)
            with open(fileNameWithPath, 'r') as in_file:
                post_data = json.load(in_file)
                tranform_items = {"temp": post_data}
                tranformed_items = transform(tranform_items,propertyDictionary)
                jsonData = tranformed_items["temp"]
                print(f"--------> {jsonData}")
                print(f"--------> {type(jsonData)}")

        return ApiExecutorRequest(apiInfoWithData, jsonData, method)

    def __str__(self):
        return "todo"


class SetCommandInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, propertyDictionary):
        parts = command.split(' ')
        # ex:
        #   set param=value
        if len(parts) < 2:
            raise ValueError("set require name=value format")
        nameValueParts = parts[1].split('=')
        name = nameValueParts[0]
        value = ""
        if len(nameValueParts) > 1:
            value = nameValueParts[1]
        # go through transformation
        value = transformValue(value,propertyDictionary)

        return SetExecutorRequest(name, value)


class ListCommandInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, propertyDictionary):
        return ListExecutorRequest()


class HelpCommandInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, propertyDictionary):
        return HelpExecutorRequest(apis)


class ManagementCommandRequestInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, propertyDictionary):
        parts = command.split(' ')
        if len(parts) < 2:
            raise ValueError(
                f"!management command requires requestName (ex: !management commands)")
        # check supported management commands
        supportedMgmtCommands = ['commands', 'apicommands','variables']
        mgmtRequest = parts[1]
        if mgmtRequest in supportedMgmtCommands:
            return ManagementCommandExecutorRequest(apis, mgmtRequest)
        raise ValueError(f"!management {mgmtRequest} is not available, supported requests for management are: {supportedMgmtCommands}")

class WaitForUserInputRequestInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, propertyDictionary):
        parts = command.split(' ')
        prompt = ""
        if len(parts) > 1:
            prompt = parts[1]
        prompt += "(press any key to):"
        return WaitForUserInputExecutorRequest(prompt)


class ExtractVariableRequestInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, propertyDictionary):
        parts = command.split(' ')
        if len(parts) < 3:
            raise ValueError(
                "!extract requires jsonPath and variable name ( ex !extract user.name userName)")
        return ExtractVariableExecutorRequest(parts[1], parts[2])


class AssertRequestInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, propertyDictionary):
        parts = command.split(' ')
        if len(parts) < 3:
            raise ValueError(
                "!assert requires variable and value ( ex !assert variable value)")
        return AssertExecutorRequest(parts[1], parts[2])

class ConvertJsonToHtmlRequestInputParser(InputParser):
    def __init__(self, workingDirectory):
        self.workingDirectory = workingDirectory

    def parseCommand(self, command, apis, propertyDictionary):
        parts = command.split(' ')
        if len(parts) < 3:
            raise ValueError(
                "!convert_json_html requires inputfile and outputfile ( ex !convert_json_html inputJsonFileName outputJsonFileName)")
        return ConvertJsonToHtmlExecutorRequest(parts[1], parts[2])
