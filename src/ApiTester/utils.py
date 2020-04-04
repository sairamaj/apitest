import sys

def readAllText(fileName):
    with open(fileName, 'r') as file:
        return file.read()


def writeAllText(fileName, content):
    with open(fileName, 'w') as file:
        file.write(content)

def read_key_value_pairs(fileName, delim):
    key_values = {}
    with open(fileName, 'r') as file:
        for line in file.readlines():
            parts = line.rstrip("\n").split(delim)
            if len(parts) > 1:
                key_values[parts[0]] = parts[1]
            else:
                key_values[parts[0]] = ""
    return key_values

if __name__ == '__main__':
    kvs = read_key_value_pairs(sys.argv[1],sys.argv[2])
    print(kvs)