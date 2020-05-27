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
                method = api["method"]
                print(api['url'])
                name = f"{method} {urlparse(url).path} {result}"
                tc = TestCase(name, result,
                              time, request, response)
                failed_message = self.check_api_status(api, self.results_path)
                if failed_message != None:
                    tc.add_failure_info(failed_message)

                test_cases.append(tc)

        ts = TestSuite("my test suite", test_cases)
        # pretty printing is on by default but can be disabled using prettyprint=False
        with open(out_file, 'w', encoding="utf-8") as f:
            TestSuite.to_file(f, [ts], prettyprint=True)

    def check_api_status(self, api, result_path):
        # check whether assert wit this id exists
        api_id = api["id"]
        http_code = api['httpcode']
        assert_file = os.path.join(
            self.results_path, f"*.{api_id}.*.assert.json")
        found_assert_files = glob.glob(assert_file)

        if len(found_assert_files) == 0:       # assert file not found
            if http_code > 299:
                return f"Failed status code is not success: {http_code}"
            else:
                return None

        # assert file found. Check the status.
        failed_message = ""
        for assert_file in found_assert_files:
            with open(assert_file, 'r') as file:
                assert_info = json.load(file)
                if assert_info['success'] == False:
                    failed_message = failed_message + \
                        " " + assert_info['message']

        if len(failed_message) > 0:
            return failed_message

        return None


if __name__ == "__main__":
    reporter = Reporter(r"C:\temp\temp\apimon\config\results\apigee")
    reporter.generate_junit(r'c:\temp\test.xml')
