import os
import json


class FilePublisher:
    def __init__(self, path):
        self.path = path
        self.counter = 1

    def send(self, data):
        parts = data.split('|')
        final_data = data[len(parts[0])+1:]
        file_name = os.path.join(self.path, f"{self.counter}.json")
        with open(file_name, 'w') as file:
            file.write(final_data)
        self.counter = self.counter+1
