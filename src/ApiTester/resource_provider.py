import os
import glob

class ResourceProvider:
    def __init__(self, resources_path):
        self.resources_path = resources_path
        self.scripts_path = os.path.join(self.resources_path, 'scripts')
        self.asserts_path = os.path.join(self.resources_path, 'asserts')
        self.variables_path = os.path.join(self.resources_path, 'variables')

    def js_filepath(self, name):
        return self.find_resoure_file(self.scripts_path, name)

    def asserts_filepath(self, name):
        return self.find_resoure_file(self.asserts_path, name)

    def variables_filepath(self, name):
        return self.find_resoure_file(self.variables_path, name)

    def api_filepath_for_http_verb(self, name, verb):
        http_path = os.path.join(self.resources_path, verb)
        return self.find_resoure_file(http_path, name)

    def find_resoure_file(self, path, name):
        file_name = f"{path}/**/{name}"
        print(file_name)
        found = glob.glob(file_name, recursive=True)
        if len(found) == 0:
            raise ValueError(f"{name} not found in {path} or its subdirectories")
        return found[0]

if __name__ == '__main__':
    print(ResourceProvider(r'c:\temp').api_filepath_for_http_verb('patch.json', 'get'))
