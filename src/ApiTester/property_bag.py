# https://www.programiz.com/python-programming/property


class PropertyBag:
    def __init__(self, parameters):
        self.properties = parameters
        self.access_token = ""
        self.session_name = "unknown"
        self.last_http_request = None
        self.resources_path = None

    def get(self, name):
        return self.properties.get(name, None)

    def add(self, name, value):
        self.properties[name] = value

    @property
    def access_token(self):
        return self._access_token

    @access_token.setter
    def access_token(self, value):
        self._access_token = value

    @property
    def session_name(self):
        return self._session_name

    @session_name.setter
    def session_name(self, value):
        self._session_name = value

    @property
    def last_http_request(self):
        return self._last_http_request

    @last_http_request.setter
    def last_http_request(self, value):
        self._last_http_request = value

    @property
    def config_filename(self):
        return self.properties["config"]

    @property
    def resources_path(self):
        return self._resources_path

    @resources_path.setter
    def resources_path(self, value):
        self._resources_path = value

    def additional_properties(self):
        additional = {}
        if self.last_http_request != None:
            additional['last_http_request.request'] = self.last_http_request.request[:30]
            additional['last_http_request.response'] = self.last_http_request.response[:30]
            additional['last_http_request.status_code'] = self.last_http_request.status_code
        additional['session'] = self.session_name
        additional['resource_path'] = self.resource_path
        return additional

            