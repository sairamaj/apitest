from http_request import HttpRequest
from jpath_extractor import JPathExtractor
from jsonpath_ng.ext import parse
from utils import line_to_dictionary
import json


class FuncEvaluator:
    def __init__(self, last_api_info):

        self.func = {
            "update_last_response": self.update_last_response,
            "remove_item_from_last_response": self.remove_item_from_last_response,
            "get_last_response": self.get_last_response
        }

        self.last_api_info = last_api_info

    def evaluate(self, name, args):

        func = self.func.get(name, None)
        if func == None:
            raise ValueError(f"{name} not found")
        print(f" args {type(args)}")
        return func(**args)

    def update_last_response(self, json_path, value=""):
        print('updating self response.')
        print(f"json_path: {json_path} new_value:{value}")
        if len(self.last_api_info.response) == 0:
            raise ValueError(
                "no last response found, call api to set response")
        response_info = json.loads(self.last_api_info.response)
        #value = JPathExtractor(response_info).extract(json_path)
        jsonpath_expr = parse(json_path)
        jsonpath_expr.find(response_info)
        jsonpath_expr.update(response_info, value)
        self.last_api_info.response = json.dumps(response_info, indent=4)
        print(self.last_api_info.response)

    def get_last_response(self):
        print('get_last_response')
        return self.last_api_info.response

    def remove_item_from_last_response(self, json_path):
        response_info = json.loads(self.last_api_info.response)
        found = response_info.get(json_path, None)
        if found == None:
            return
        response_info.pop(json_path)
        self.last_api_info.response = json.dumps(response_info, indent=4)
        print(self.last_api_info.response)


def evaluate_func(command, last_request_info):
    command = command.strip().strip('__')
    args = line_to_dictionary(command)
    func_name = next(iter(args))    # first key is name
    args.pop(func_name, None)       # remove this
    return FuncEvaluator(last_request_info).evaluate(func_name, args)
