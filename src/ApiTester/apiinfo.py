class ApiInfo:
    def __init__(self, api, route, path, baseUrl, body, headers):
        self.api = api
        self.route = route
        self.path = path
        self.baseUrl = baseUrl
        self.body = body
        self.headers = headers
        if self.headers == None:
            self.headers = {}   # create empty one.``
        if self.body == None:
            self.body = {}
