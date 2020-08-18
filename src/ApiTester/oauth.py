import base64
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
        print('getaccess token')
        for k, v in self.apiInfo.headers.items():
            headers[k] = self.base64Value(v)
        print(f"headers {headers}")
        url = self.apiInfo.baseUrl + self.apiInfo.path
        response = requests.post(url, self.apiInfo.body, verify=False,headers=headers)
        self.response = response
        if response.status_code == 200:
            return response.json()
        raise ApiException(response.status_code, response.content)

    def base64Value(self,val):
        print(f"val : {val}")
        pos = val.find("base64")
        print(f"pos: {pos}")
        if pos < 0 :
            return val
        inputVal = val[pos+len("base64"):].strip()
        print(type(inputVal))
        encodedBytes = base64.b64encode(inputVal.encode("utf-8"))
        encodedStr = str(encodedBytes, "utf-8")
        return str(val[0:pos]) + encodedStr

