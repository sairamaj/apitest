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
    commands.append(CommandInfo("!assert", "asserts a json_path with expected value"))
    commands.append(CommandInfo("!asserts_with_js", "asserts file records using java script file."))
    commands.append(CommandInfo("!extract", "extracts to a variable using jsonpath from last response"))
    commands.append(CommandInfo("!extract_from_request", "extracts from request"))
    commands.append(CommandInfo("!js", "executes javascript file( ex: !js sample.js)"))
    commands.append(CommandInfo("!set", "sets a variable to value (ex: !set name=value)"))
    commands.append(CommandInfo("!setgroup", "set group variable"))
    commands.append(CommandInfo("!list", "lists all variables"))
    commands.append(CommandInfo("!print", "prints a message (ex: !print message here)"))
    commands.append(CommandInfo("!return", "skips the following lines in scenario"))
    commands.append(CommandInfo("!convert_json_html", "converts json to html"))
    commands.append(CommandInfo("!waitforuserinput", "waits for user input"))
    return commands

def getCommandsInfo():
    info = []
    for cmd in getCommands():
        info.append(cmd.to_dict())
    return info

if __name__ == '__main__':
    print(getCommandsInfo())