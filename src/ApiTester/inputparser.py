import sys

class InputParser:
    def __init__(self):
        self.method = 'get'
        self.fileName = ''
        self.route = ''
        self.path = ''

    def parse(self,command):
        parts = command.split(' ')
        if len(parts) > 1:
            self.method = parts[1]
            if len(parts) > 2 :
                self.fileName = parts[2]
        parts = parts[0].split('.')
        if len(parts) > 1:
            self.route = parts[0]
            self.path = parts[1]
        else:       
            self.route = parts[0]
            self.path = "_"

if __name__ == "__main__":
    parser = InputParser()
    print(sys.argv[1])

    parser.parse(sys.argv[1])
    print(f"route: {parser.route}")
    print(f"path: {parser.path}")
    print(f"method: {parser.method}")
    print(f"fileName: {parser.fileName}")
