from pprint import pprint
from string import Template
from ui import getUserInput

def readAllText(fileName):
    with open(fileName, "r") as file:
        return file.read()

def transform(inputs, argItems):

    for k,v in inputs.items():
        inputs[k] = str(v)  
    
    for k,v in inputs.items():
        if '$userinput' in v and k in argItems:
            t = Template(str(v))
            userinput = argItems[k]
            inputs[k] = t.substitute(userinput=userinput)

    for k,v in inputs.items():
        if '$userinput' in v:
            t = Template(str(v))
            userinput = getUserInput(f"{k}:")
            inputs[k] = t.substitute(userinput=userinput)
    return inputs
