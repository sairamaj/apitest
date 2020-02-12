import requests
from exceptions import ApiException
from pprint import pprint

class Api:
    def __init__(self, url,access_token, data):
        self.url = url
        self.access_token = access_token
        self.data = data

    def get(self):
        headers={'Content-Type':'application/json',
                'Authorization': 'Bearer {}'.format(self.access_token)}
        pprint(headers)
        result = requests.get(self.url, headers=headers, verify=False)
        if result.status_code == 200:
            return result.json()
        raise ApiException(result.status_code, result.content)
    

