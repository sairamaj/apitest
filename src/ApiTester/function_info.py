class FunctionInfo:
    def __init__(self, name, description):
        self.name = name
        self.description = description
    
    def to_dict(self):
        return {
            "name" : self.name,
            "description" : self.description
        } 

def getFunctions():
    functions = []
    functions.append(FunctionInfo("update_item_in_last_response", "updates last response given json path and value"))
    functions.append(FunctionInfo("add_item_to_last_response", "add item to last response given json path and value"))
    functions.append(FunctionInfo("remove_item_from_last_response", "removes item from last response given json path"))
    functions.append(FunctionInfo("get_last_response", "gets last response"))
    return functions

def getFunctionsInfo():
    info = []
    for cmd in getFunctions():
        info.append(cmd.to_dict())
    return info

if __name__ == '__main__':
    print(getCommandsInfo())