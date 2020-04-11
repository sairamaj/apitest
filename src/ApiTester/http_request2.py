import requests

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
        response = requests.get(self.url, headers=self.headers, verify=False)
        return response

