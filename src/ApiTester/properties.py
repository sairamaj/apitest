# https://www.programiz.com/python-programming/property

class Properties:
    def __init__(self, parameters):
        self.properties = parameters
        self.access_token = ""
        self.session_name = "unknown"

    def get(self, name):
        return self.properties.get(name, None)
    
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
