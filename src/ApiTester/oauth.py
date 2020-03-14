import requests
from exceptions import ApiException
from pprint import pprint
from requests.packages.urllib3.exceptions import InsecureRequestWarning

requests.packages.urllib3.disable_warnings(InsecureRequestWarning)

class OAuth:
    def __init__(self, apiInfo):
        """Initializes with url ad parameters."""
        self.apiInfo = apiInfo
        self.response = None

    def getAccessToken(self):
        """Posts to the url and gets the resonse json."""
        headers = {}
        headers = dict(headers, **self.apiInfo.headers)
        url = self.apiInfo.baseUrl + self.apiInfo.path
        response = requests.post(url, self.apiInfo.body, verify=False,headers=headers)
        self.response = response
        if response.status_code == 200:
            return response.json()
        raise ApiException(response.status_code, response.content)
    

