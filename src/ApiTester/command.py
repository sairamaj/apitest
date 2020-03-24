from oauth import OAuth
from apiresponse import ApiResponse
from pprint import pprint
from api import Api
import json
from executors import AccessTokenExecutor, ApiExecutor, HelpExecutor, SetExecutor, ListPropertiesExecutor, ManagementCommandExecutor
from executors import WaitForUserInputCommandExecutor
from abc import ABCMeta, abstractstaticmethod


class Command:
    def __init__(self, property_bag):
        self.commands = {'accesstoken': AccessTokenExecutor(property_bag),
                         'api': ApiExecutor(property_bag),
                         '!set': SetExecutor(property_bag),
                         '!list': ListPropertiesExecutor(property_bag),
                         "!help": HelpExecutor(property_bag),
                         "!management": ManagementCommandExecutor(property_bag),
                         "!waitforuserinput": WaitForUserInputCommandExecutor(property_bag)
                         }

    def execute(self, executorRequest):
        if executorRequest.command == 'api':
            # try geting by specific api.
            executor = self.commands.get(executorRequest.apiInfo.api, None)
            if executor == None:
                executor = self.commands.get(
                    executorRequest.command, None)    # then go to generic api
        else:
            executor = self.commands.get(executorRequest.command, None)
        executor.execute(executorRequest)
