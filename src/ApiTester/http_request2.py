import requests
import json

class HttpRequest2:
    def __init__(self, url, method, headers, body):
        self.url = url
        self.method = method
        self.headers = headers
        self.body = body

    def get(self):
        print('_________________')
        print(self.body)
        print('_________________')
        if self.method == 'get':
            response = requests.get(self.url, headers=self.headers, verify=False)
        elif self.method == 'post':
            response = requests.post(self.url, data=self.body, headers=self.headers, verify=False)
        elif self.method == 'patch':
            response = requests.post(self.url, data=self.body, headers=self.headers, verify=False)
        else:
            raise ValueError(f"{self.method} not supported")

        return response

