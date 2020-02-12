from oauth import OAuth
from apiresponse import ApiResponse
from pprint import pprint
from api import Api

class Command:
    def __init__(self):
        self.apiResponse = ApiResponse()
        self.commands = {'accesstoken': self.accessToken, 'tips': self.executeApi}

    def execute(self, command, url, data):
        executor = self.commands.get(command, None)
        if executor == None:
            raise ValueError(f"{command} not found.")
        executor(url, data)

    def accessToken(self, url, data):
        oauth = OAuth(url, data)
        response = oauth.getAccessToken()
        pprint(response)
        self.apiResponse.access_token = response["access_token"]
        pprint(self.apiResponse.access_token)

    def executeApi(self, url, data):
        api = Api(url, self.apiResponse.access_token, data)
        response = api.get()
        pprint(response)

