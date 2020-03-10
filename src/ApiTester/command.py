from oauth import OAuth
from apiresponse import ApiResponse
from pprint import pprint
from api import Api
from logcollector import collectlog
import json


class Command:
    def __init__(self):
        self.apiResponse = ApiResponse()
        self.commands = {'accesstoken': self.accessToken}

    def execute(self, apiInfo, json = json, method = 'get'):
        executor = self.commands.get(apiInfo.api, None)
        if executor == None:
            # raise ValueError(f"{command} not found.")
            if method == 'get':
                executor = self.executeApi
            else:
                executor = self.executeApiPost
        
        executor(apiInfo, json)

    def accessToken(self, apiInfo, json):
        oauth = OAuth(apiInfo)
        try:
            response = oauth.getAccessToken()
            collectlog(oauth.response)
            pprint(response)
            self.apiResponse.access_token = response["access_token"]
        except Exception as e:
            collectlog(oauth.response)
            raise

    def executeApi(self, apiInfo, json):
        api = Api(apiInfo, self.apiResponse.access_token)
        try:
            response = api.get()
            collectlog(api.response)
            pprint(response)
        except Exception as e:
            collectlog(api.response)
            raise
        finally:
            pass
    def executeApiPost(self, apiInfo, json):
        api = Api(apiInfo, self.apiResponse.access_token)
        try:
            response = api.post(json)
            collectlog(api.response)
            pprint(response)
        except Exception as e:
            collectlog(api.response)
            raise
        finally:
            pass
