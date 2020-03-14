import sys
import json
from apiinfo import ApiInfo


class Config:
    def __init__(self, config):
        self.config = json.loads(self.readAllText(config))

    def get(self, name):
        val = self.config.get(name, None)
        if val == None:
            raise ValueError(f"{name} not found")
        return val

    def readAllText(self, fileName):
        with open(fileName, 'r') as file:
            return file.read()

    def apis(self):
        apiInfos = {}
        for item in self.config:
            paths = {}
            baseUrl = ''
            for route in self.config[item]:
                if route in ['baseurl']:
                    baseUrl = self.config[item][route]
                else:
                    path = self.config[item][route].get('path', None)
                    paths[route] = ApiInfo(item, route,  path,  baseUrl,
                        self.config[item][route].get('body', None), 
                        self.config[item][route].get('headers', None))
            apiInfos[item] = paths
        return apiInfos


if __name__ == "__main__":
    config = Config(sys.argv[1])
    for name, apiInfos in config.apis().items():
        print('name', name)
        print('____________')
        print(apiInfos)
        print('____________')
        print(apiInfos)
        for _,apiInfo in apiInfos.items():
            print(f"apinfo: {apiInfo}")
            print(f"\t{apiInfo.api}")
            print(f"\t\t{apiInfo.body}")
            print(f"\t\t{apiInfo.headers}")
