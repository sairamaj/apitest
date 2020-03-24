import os
import sys
import json
from apiinfo import ApiInfo
from abc import ABCMeta, abstractstaticmethod
from executorRequest import ApiExecutorRequest, SetExecutorRequest, ListExecutorRequest, HelpExecutorRequest 
from executorRequest import ManagementCommandExecutorRequest, WaitForUserInputExecutorRequest, ExtractVariableExecutorRequest
from transform import transform
from transform import transformString

def parseCommand(command, workingDirectory, apis, propertyDictionary):
    commands = {'!list': ListCommandInputParser(workingDirectory),
                '!set': SetCommandInputParser(workingDirectory),
                '!help': HelpCommandInputParser(workingDirectory),
                '!management': ManagementCommandRequestInputParser(workingDirectory),
                '!waitforuserinput': WaitForUserInputRequestInputParser(workingDirectory),
                '!extract': ExtractVariableRequestInputParser(workingDirectory)
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

        apiInfos = apis.get(route, None)

        if apiInfos == None:
            raise ValueError(f"{route} not found")
        else:
            foundApiInfo = apiInfos.get(path, None)
            if foundApiInfo == None:
                raise ValueError(f"{route}.{path} not found")

        data = transform(foundApiInfo.body, propertyDictionary)
        path = transformString('path', foundApiInfo.path, propertyDictionary)
        baseUrl = transformString(
            'baseurl', foundApiInfo.baseUrl, propertyDictionary)

        transformedHeaders = transform(
            foundApiInfo.headers, propertyDictionary)

        apiInfoWithData = ApiInfo(
            foundApiInfo.api, foundApiInfo.route, path, baseUrl, data, transformedHeaders)
        jsonData = ""
        if method.lower() == 'post':
            if len(filename) == 0:
                raise Exception('post requires filename')
            fileNameWithPath = os.path.join(self.workingDirectory, filename)
            with open(fileNameWithPath, 'r') as in_file:
                jsonData = json.load(in_file)

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
        if len(nameValueParts) > 1:
            value = nameValueParts[1]
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
            raise ValueError(f"!management command requires requestName (ex: !management commands)")
        return ManagementCommandExecutorRequest(apis, parts[1])

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
            raise ValueError("!extract requires jsonPath and variable name ( ex !extract user.name userName)")
        return ExtractVariableExecutorRequest(parts[1], parts[2])        