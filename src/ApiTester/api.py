import requests
from exceptions import ApiException
from pprint import pprint

class Api:
    def __init__(self, apiInfo, access_token):
        self.apiInfo = apiInfo
        self.access_token = access_token
        self.response = None

    def get(self):
        headers = {'Content-Type': 'application/json',
                   'Authorization': 'Bearer {}'.format(self.access_token)}
        headers = dict(headers, **self.apiInfo.headers)
        url = self.apiInfo.baseUrl + self.apiInfo.path
        response = requests.get(
            url, headers=headers, verify=False)
        self.response = response
        if response.status_code == 200:
            return response.json()
        raise ApiException(response.status_code, response.content)
