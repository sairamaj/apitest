import os
import json

last_data_id = 1


class FilePublisher:
    def __init__(self, path):
        self.path = path
        if os.path.exists(path) == False:
            os.makedirs(path, exist_ok=True)

    def api_data(self, data, id):
        self.write(data, 'api', id)

    def assert_data(self, data, id, success):
        self.write(data, 'assert', f"{id}.{success}")

    def extract_data(self, data):
        self.write(data, 'extract')

    def error_data(self, data):
        self.write(data, 'error')

    def js_data(self, data):
        self.write(data, 'js')

    def write(self, data, name, id=None):
        global last_data_id
        parts = data.split('|')
        final_data = data[len(parts[0])+1:]
        formatted_data = json.dumps(json.loads(final_data), indent=4)
        if id == None:
            file_name = os.path.join(self.path, f"{last_data_id}.{name}.json")
        else:
            file_name = os.path.join(
                self.path, f"{last_data_id}.{id}.{name}.json")
        with open(file_name, 'w') as file:
            file.write(formatted_data)
        last_data_id = last_data_id+1
