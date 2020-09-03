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
        print(self.url)
        print(self.headers)
        print(f'|{self.body}|')
        print('_________________')
        if self.method == 'get':
            response = requests.get(self.url, headers=self.headers, verify=False,data=json.dumps({}, indent=4))
        elif self.method == 'post':
            print('posing...')
            response = requests.post(self.url, data=self.body, headers=self.headers, verify=False)
        elif self.method == 'patch':
            response = requests.patch(self.url, data=self.body, headers=self.headers, verify=False)
        else:
            raise ValueError(f"{self.method} not supported")

        print('_________________')
        print(response)
        print(response.content)
        print(response.headers)
        print('_________________')
        return response

