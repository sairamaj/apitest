import os
import sys
import json
from apiinfo import ApiInfo
from abc import ABCMeta, abstractstaticmethod
from executorRequest import ApiExecutorRequest, SetExecutorRequest, ListExecutorRequest
from transform import transform
from transform import transformString


def parseCommand(command, workingDirectory, apis, propertyDictionary):
    parts = command.split(' ')

    parser = None
    if parts[0] == 'quit' or parts[0][0] == 'q':
        return None
    if parts[0] == 'set':
        parser = SetCommandInputParser(workingDirectory)
    elif parts[0] == 'list':
        parser = ListCommandInputParser(workingDirectory)
    else:
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


if __name__ == "__main__":
    print(sys.argv[1:])
    parser = parseCommand(' '.join(sys.argv[1:]))
    print(str(parser))
