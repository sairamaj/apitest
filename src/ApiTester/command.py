from oauth import OAuth
from apiresponse import ApiResponse
from pprint import pprint
from api import Api
import json
from executors import AccessTokenExecutor, ApiExecutor, HelpExecutor, SetExecutor, ListPropertiesExecutor, ManagementCommandExecutor
from abc import ABCMeta, abstractstaticmethod


class Command:
    def __init__(self, properties):
        self.commands = {'accesstoken': AccessTokenExecutor(properties),
                         'api': ApiExecutor(properties),
                         'set': SetExecutor(properties),
                         'list': ListPropertiesExecutor(properties),
                         "help": HelpExecutor(properties),
                         "!management": ManagementCommandExecutor(properties)}

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
