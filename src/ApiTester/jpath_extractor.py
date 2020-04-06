import json
import sys
from jsonpath_ng.ext import parse
from utils import readAllText

class JPathExtractor:
    def __init__(self, data):
        self.data = data

    def extract(self, jsonpath):
        extracted_value = ''
        jsonpath_expr = parse(jsonpath)
        matches = jsonpath_expr.find(self.data)
        if len(matches) == 0:
            raise ValueError(f"JPathExtractor.extract no match found for {jsonpath}")
        for match in matches:
            if type(match.value) is list:
                if len(match.value) > 1:
                    raise ValueError(
                        f"JPathExtractor.extract found multiple items found for{jsonpath} count: {len(match.value)}")
                elif len(match.value) == 0:
                    raise ValueError(
                        f"JPathExtractor.extract no value found for {jsonpath}")
                else:
                    extracted_value = match.value[0]
            else:
                extracted_value = match.value

        return extracted_value

if __name__  == "__main__":
    value = JPathExtractor(json.loads(readAllText(sys.argv[1]))).extract(sys.argv[2])
    print(f"value is:{value}")