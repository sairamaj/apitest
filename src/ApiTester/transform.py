from pprint import pprint
from string import Template
from ui import getUserInput
import re
import pystache


def readAllText(fileName):
    with open(fileName, 'r') as file:
        return file.read()

def getUserInput(prompt):
    print(prompt + ":", end='')
    return input()

def updateVariables(inputs, getFunc):
    for k,v in inputs.items():
        variables = re.findall(r"{(\w+)}", v)
        variableValues = {}
        for variable in variables:
            val = getFunc(variable)
            if val != None:
                variableValues[variable] = val
        
        if len(variableValues) > 0:
            inputs[k] = pystache.render(str(v),variableValues)
    return inputs

def transform(inputs, argItems):
    if inputs == None:
        return None
    for k,v in inputs.items():
        inputs[k] = str(v)  
    inputs = updateVariables(inputs, lambda v: argItems.get(v,None))
    inputs = updateVariables(inputs, getUserInput)  
    return inputs
