import requests
from exceptions import ApiException
from pprint import pprint

class Api:
    def __init__(self, url,access_token, data):
        self.url = url
        self.access_token = access_token
        self.data = data
        self.response = {}

    def get(self):
        headers={'Content-Type':'application/json',
                'Authorization': 'Bearer {}'.format(self.access_token)}
        pprint(headers)
        response = requests.get(self.url, headers=headers, verify=False)
        self.request = response
        if response.status_code == 200:
            return response.json()
        raise ApiException(response.status_code, response.content)
    

