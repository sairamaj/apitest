import os
import glob
import json
from junit_xml import TestSuite, TestCase
from urllib.parse import urlparse


class Reporter:
    def __init__(self, results_path):
        self.results_path = results_path

    def generate_junit(self, out_file):
        api_jsons = os.path.join(self.results_path, "*api*.json")

        test_cases = []
        for response_file in glob.glob(api_jsons):
            with open(response_file, 'r') as file:
                api = json.load(file)
                url = api['url']
                http_code = api['httpcode']
                result = f"{http_code}({api['statuscode']})"
                time = api["timetaken"]/1000
                request = api['request']
                response = api['response']
                print(api['url'])
                tc = TestCase(urlparse(url).path, result,
                              time, request, response)
                if http_code > 299:
                    tc.add_failure_info("*FAILED*")
                test_cases.append(tc)

        ts = TestSuite("my test suite", test_cases)
        # pretty printing is on by default but can be disabled using prettyprint=False
        with open(out_file, 'w') as f:
            TestSuite.to_file(f, [ts], prettyprint=True)


if __name__ == "__main__":
    reporter = Reporter(r"C:\temp\temp\apimon\config\results\apigee")
    reporter.generate_junit(r'c:\temp\test.xml')