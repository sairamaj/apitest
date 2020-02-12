import requests
from exceptions import ApiException
from pprint import pprint

class OAuth:
    def __init__(self, url,bodyParameters):
        """Initializes with url ad parameters."""
        self.url = url
        self.bodyParameters = bodyParameters
        self.response = None

    def getAccessToken(self):
        """Posts to the url and gets the resonse json."""
        response = requests.post(self.url, self.bodyParameters, verify=False)
        self.response = response
        if response.status_code == 200:
            return response.json()
        raise ApiException(response.status_code, response.content)
    

