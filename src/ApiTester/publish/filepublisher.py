import os
import json


class FilePublisher:
    def __init__(self, path):
        self.path = path
        self.counter = 1

    def api_data(self, data):
        self.write(data, 'api')

    def assert_data(self, data):
        self.write(data, 'assert')

    def extract_data(self, data):
        self.write(data, 'extract')

    def error_data(self, data):
        self.write(data, 'error')

    def js_data(self, data):
        self.write(data, 'js')

    def write(self, data, name):
        parts = data.split('|')
        final_data = data[len(parts[0])+1:]
        formatted_data = json.dumps(json.loads(final_data), indent=4)
        file_name = os.path.join(self.path, f"{self.counter}.{name}.json")
        with open(file_name, 'w') as file:
            file.write(formatted_data)
        self.counter = self.counter+1
