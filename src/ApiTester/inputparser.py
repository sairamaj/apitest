import sys
from abc import ABCMeta, abstractstaticmethod


def parseCommand(command):
    parts = command.split(' ')
    if parts[0] == 'quit' or parts[0][0] == 'q':
        return None
    if parts[0] == 'set':
        parser = SetCommandInputParser()
    else:
        parser = ApiCommandInputParser()
    parser.parseCommand(command)
    return parser


class InputParser:
    def __init__(self):
        self.command = ''

    @abstractstaticmethod
    def parseCommand(command):
        """The required parse method which all command obejcts will use"""

class ApiCommandInputParser(InputParser):
    def __init__(self):
        self.command = 'api'
        self.method = 'get'
        self.filename = ''
        self.route = ''
        self.path = ''

    def parseCommand(self, command):
        parts = command.split(' ')
        # ex:
        #   users
        #   users post file
        if len(parts) > 1:
            self.method = parts[1]
            if len(parts) > 2:
                self.filename = parts[2]
        parts = parts[0].split('.')
        if len(parts) > 1:
            self.route = parts[0]
            self.path = parts[1]
        else:
            self.route = parts[0]
            self.path = "_"
        if len(self.method.strip()) == 0 :
            self.method = "get"

    def __str__(self):
        return f"command:{self.command} route:{self.route} path:{self.path} method:{self.method} filename:{self.filename}"

class SetCommandInputParser(InputParser):
    def __init__(self):
        self.command = 'set'
        self.name = ''
        self.value = ''

    def parseCommand(self, command):
        parts = command.split(' ')
        # ex:
        #   set param=value
        if len(parts) < 2:
            raise ValueError("set require name=value format")
        nameValueParts = parts[1].split('=')
        self.name = nameValueParts[0]
        if len(nameValueParts) > 1:
            self.value = nameValueParts[1]

    def __str__(self):
        return f"command:{self.command} parameterName:{self.name} parameterValue:{self.value}"


if __name__ == "__main__":
    print(sys.argv[1:])
    parser = parseCommand(' '.join(sys.argv[1:]))
    print(str(parser))
