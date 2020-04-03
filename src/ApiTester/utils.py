
def readAllText(fileName):
    with open(fileName, 'r') as file:
        return file.read()


def writeAllText(fileName, content):
    with open(fileName, 'w') as file:
        file.write(content)

