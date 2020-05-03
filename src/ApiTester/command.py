from oauth import OAuth
from apiresponse import ApiResponse
from pprint import pprint
from api import Api
import json
from executors import AccessTokenExecutor, ApiExecutor, HelpExecutor, SetExecutor, SetGroupExecutor, \
    ListPropertiesExecutor, ManagementCommandExecutor
from executors import WaitForUserInputCommandExecutor, ExtractVariableCommandExecutor, AssertCommandExecutor
from executors import JavaScirptCommandExecutor, AssertsJsRequestCommandExecutor, ConvertJsonToHtmlCommandExecutor, \
    PrintCommandExecutor
from executors import HttpRequestCommandExecutor, FuncCommandExecutor
from abc import ABCMeta, abstractstaticmethod


class Command:
    def __init__(self, property_bag):
        self.commands = {'accesstoken': AccessTokenExecutor(property_bag),
                         'api': ApiExecutor(property_bag),
                         '!set': SetExecutor(property_bag),
                         '!setgroup': SetGroupExecutor(property_bag),
                         '!list': ListPropertiesExecutor(property_bag),
                         "!help": HelpExecutor(property_bag),
                         "!httprequest": HttpRequestCommandExecutor(property_bag),
                         "!management": ManagementCommandExecutor(property_bag),
                         "!print": PrintCommandExecutor(property_bag),
                         "!waitforuserinput": WaitForUserInputCommandExecutor(property_bag),
                         "!extract": ExtractVariableCommandExecutor(property_bag),
                         "!js": JavaScirptCommandExecutor(property_bag),
                         "!assert": AssertCommandExecutor(property_bag),
                         "!asserts_with_js": AssertsJsRequestCommandExecutor(property_bag),
                         "!convert_json_html": ConvertJsonToHtmlCommandExecutor(property_bag),
                         "__funceval__": FuncCommandExecutor(property_bag)
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
