import requests
from exceptions import ApiException
from pprint import pprint

class Api:
    def __init__(self, url,access_token, data,headers):
        self.url = url
        self.access_token = access_token
        self.data = data
        self.headers = headers
        if self.headers == None:
            self.headers = {}       # Create empty one.
        self.response = None

    def get(self):
        headers={'Content-Type':'application/json',
                'Authorization': 'Bearer {}'.format(self.access_token)}
        headers = dict(headers, **self.headers)                
        pprint(headers)
        response = requests.get(self.url, headers=headers, verify=False)
        self.response = response
        if response.status_code == 200:
            return response.json()
        raise ApiException(response.status_code, response.content)
    

