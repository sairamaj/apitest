import json


class ApiInfo(dict):
    def __init__(self, api, route, path, baseUrl, body, headers):
        dict.__init__(self, path=f"{route}.{path}")
        self.api = api
        self.route = route
        self.path = path
        self.baseUrl = baseUrl
        self.body = body
        self.headers = headers
        if self.headers == None:
            self.headers = {}   # create empty one.``
