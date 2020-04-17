import requests
from exceptions import ApiException
from pprint import pprint
import json


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
        respone_json = ""
        if len(response.content) > 0 :
                respone_json = response.json()
        if response.status_code == 200:
            return respone_json
        raise ApiException(response.status_code, response)

    def post(self, jsonData):
        headers = {'Content-Type': 'application/json',
                   'Authorization': 'Bearer {}'.format(self.access_token)}
        headers = dict(headers, **self.apiInfo.headers)
        url = self.apiInfo.baseUrl + self.apiInfo.path
        response = requests.post(
            url, data=json.dumps(jsonData, indent=4), headers=headers, verify=False)
        self.response = response
        if response.status_code == 200:
            if len(response.content) > 0 :
                return response.json()
            return ""
        
        raise ApiException(response.status_code, response.content)

    def put(self, jsonData):
        headers = {'Content-Type': 'application/json',
                   'Authorization': 'Bearer {}'.format(self.access_token)}
        headers = dict(headers, **self.apiInfo.headers)
        url = self.apiInfo.baseUrl + self.apiInfo.path
        response = requests.put(
            url, data=json.dumps(jsonData, indent=4), headers=headers, verify=False)
        self.response = response
        if response.status_code == 200:
            if len(response.content) > 0 :
                return response.json()
            return ""
        
        raise ApiException(response.status_code, response.content)

    def patch(self, jsonData):
        headers = {'Content-Type': 'application/json',
                   'Authorization': 'Bearer {}'.format(self.access_token)}
        headers = dict(headers, **self.apiInfo.headers)
        url = self.apiInfo.baseUrl + self.apiInfo.path
        response = requests.patch(
            url, data=json.dumps(jsonData, indent=4), headers=headers, verify=False)
        self.response = response
        if response.status_code == 200:
            if len(response.content) > 0 :
                return response.json()
            return ""
        raise ApiException(response.status_code, response.content)

    def delete(self, jsonData):
        headers = {'Content-Type': 'application/json',
                   'Authorization': 'Bearer {}'.format(self.access_token)}
        headers = dict(headers, **self.apiInfo.headers)
        url = self.apiInfo.baseUrl + self.apiInfo.path
        response = requests.delete(
            url, data=json.dumps(jsonData, indent=4), headers=headers, verify=False)
        self.response = response
        if response.status_code == 200:
            if len(response.content) > 0 :
                return response.json()
            return ""
        raise ApiException(response.status_code, response.content)
