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

    def execute(self, command, url, data):
        executor = self.commands.get(command, None)
        if executor == None:
            # raise ValueError(f"{command} not found.")
            executor = self.executeApi 
        executor(url, data)

    def accessToken(self, url, data):
        oauth = OAuth(url, data)
        try:
            response = oauth.getAccessToken()
            collectlog(oauth.response)
            pprint(response)
            self.apiResponse.access_token = response["access_token"]
        except Exception as e:
            collectlog(oauth.response)
            raise

    def executeApi(self, url, data):
        api = Api(url, self.apiResponse.access_token, data)
        try:
            response = api.get()
            collectlog(api.response)
            pprint(response)
        except Exception as e:
            collectlog(api.response)
            raise
        finally:
            pass

