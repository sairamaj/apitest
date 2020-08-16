from pprint import pprint
from string import Template
from ui import getUserInput
from utils import writeAllText, readAllText
import re
import pystache
import sys
import json
from variables.evaluate_dynamic_varb import evaluate_dynamic


def getUserInput(prompt):
    print(prompt + ":", end='')
    return input()


def getEmpty(varb):
    return ""


def updateVariables(inputs, getFunc):
    for k, v in inputs.items():
        if type(v) is str:
            variables1 = re.findall(r"{{{(.+?)}}", v)
            variables = re.findall(r"{{(.+?)}}", v)
            variables.extend(variables1)
            variableValues = {}
            for variable in variables:
                if variable.startswith('$'):
                    val = evaluate_dynamic(variable)
                    variableValues[variable] = val
                else:
                    val = getFunc(variable)
                    if val != None:
                        subItems = {'temp': val}
                        updatedSubItems = updateVariables(subItems, getFunc)
                        variableValues[variable] = updatedSubItems.get('temp')
            if len(variableValues) > 0:
                inputs[k] = pystache.render(str(v), variableValues)
        if type(v) is dict:
            inputs[k] = updateVariables(v, getFunc)
        if type(v) is list:
            newItems = []
            for item in v:
                newItems.append(updateVariables(item, getFunc))
            inputs[k] = newItems
    return inputs


def transform(inputs, argItems, user_input):
    if inputs == None:
        return None
    inputs = updateVariables(inputs, lambda v: argItems.get(v, None))
    if user_input == True:
        inputs = updateVariables(inputs, getUserInput)
    else:
        inputs = updateVariables(inputs, getEmpty)
    return inputs


def transformString(name, item, argItems, user_input):
    if item == None:
        return None
    inputs = {name: item}
    inputs = updateVariables(inputs, lambda v: argItems.get(v, None))
    if user_input == True:
        inputs = updateVariables(inputs, getUserInput)
    else:
        inputs = updateVariables(inputs, getEmpty)
    return inputs[name]


def transformValue(value, argItems):
    # create temp dict
    temp = {"key": value}
    transform(temp, argItems,True)
    # extract value
    return temp['key']


def getVariables(data):
    return re.findall(r"{(\w+)}", data)

def getVariablesFromDict(data):
    variables = []
    if data == None:
        return variables
    for k,v in data.items():
        itemVariables = getVariables(v)
        for variable  in itemVariables:
            variables.append(variable)
    return variables

if __name__ == "__main__":
    data = """
{
	"test" : null,
	"email": "{{email}}",
	"firstName": "sairama2",
	"lastName": "jamal3",
	"userName": "saij2",
	"attributes": [
		{
			"name": "{{attr1}}",
			"value": "val1"
		},
		{
			"name": "{{attr2}}",
			"value": "val2"
		}
    ],
    "user":{
        "firstname" : "{{first_name}}",
        "lastname": "{{last_name}}",
        "address" : "address here"
    }
}
"""

    info = json.loads(data)
    variables = {
        "email": "s@abc.com",
        "attr1": "attribute_1",
        "attr2": "attribute_2",
        "first_namex": "myfirstname",
        "last_name": "mylastname"
    }
    output = transform(info, variables)
    print(output)
    print('_______________')
    print(json.dumps(output))
    writeAllText(r'c:\temp\test2.json', json.dumps(output, indent=4))
    print('_______________')
