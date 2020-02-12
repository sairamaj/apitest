def readAllText(fileName):
    with open(fileName, "r") as file:
        return file.read()


def transform(parameters, inputItems):
    # Replace with command line input
    for k, v in inputItems.items():
        parameters[k] = v

    # Ask the user for inputs if any after overwriting with command line inputs.
    for k, v in parameters.items():
        if v == "<userinput>":
            parameters[k] = input(f"{k}:")
        elif v == "<fileinput>":
            parameters[k] = readAllText(input(f"{k} (filename):"))
    return parameters
