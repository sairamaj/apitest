class ApiInfo:
    def __init__(self, api, route, path, baseUrl, body, headers):
        print(f"  api {api}")
        print(f"  route {route}")
        print(f"  path {path}")
        self.api = api
        self.route = route
        self.path = path
        self.baseUrl = baseUrl
        self.body = body
        self.headers = headers
        if self.headers == None:
            self.headers = {}   # create empty one.``
