import json

class Config:
    def __init__(self,config):
        self.config = json.loads(self.readAllText(config))

    def get(self,name):
        val = self.config.get(name, None)
        print(f"val is: ${val}")
        if val == None:
            raise ValueError(f"{name} not found")
        return val

    def readAllText(self,fileName):
        with open(fileName, 'r') as file:
            return file.read()
    
    def apis(self):
        return list(self.config.keys())

