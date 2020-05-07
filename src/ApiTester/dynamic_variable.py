class DynamicVariableInfo:
    def __init__(self, name, description):
        self.name = name
        self.description = description
    
    def to_dict(self):
        return {
            "name" : self.name,
            "description" : self.description
        } 

def getDynamicVariables():
    varaibles = []
    varaibles.append(DynamicVariableInfo("random", "gets random value between given optional min and max (defaults: min=1, max=1000"))
    varaibles.append(DynamicVariableInfo("today_date", "gets today's date optional format and with adding/subtracting days(defaults: days=0"))
    varaibles.append(DynamicVariableInfo("guid", "gets new guid"))
    return varaibles

def getDynamicVariablesInfo():
    info = []
    for variable in getDynamicVariables():
        info.append(variable.to_dict())
    return info

if __name__ == '__main__':
    print(getDynamicVariablesInfo())