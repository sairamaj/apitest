class CommandInfo:
    def __init__(self, name, description):
        self.name = name
        self.description = description
    
    def to_dict(self):
        return {
            "name" : self.name,
            "description" : self.description
        } 

def getCommands():
    commands = []
    commands.append(CommandInfo("!assert", "asserts a variable with expected value"))
    commands.append(CommandInfo("!extract", "extracts to a variable using jsonpath from last response"))
    commands.append(CommandInfo("!js", "executes javascript file"))
    commands.append(CommandInfo("!set", "sets a variable to value"))
    commands.append(CommandInfo("!list", "lists all variables"))
    commands.append(CommandInfo("!convert_json_html", "converts json to html"))
    commands.append(CommandInfo("!management", "management apis"))
    commands.append(CommandInfo("!waitforuserinput", "waits for user input"))
    return commands

def getCommandsInfo():
    info = []
    for cmd in getCommands():
        info.append(cmd.to_dict())
    return info

if __name__ == '__main__':
    print(getCommandsInfo())