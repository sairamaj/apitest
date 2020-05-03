import os


class ResourceProvider:
    def __init__(self, resources_path):
        self.resources_path = resources_path
        self.scripts_path = os.path.join(self.resources_path, 'scripts')
        self.asserts_path = os.path.join(self.resources_path, 'asserts')
        self.variables_path = os.path.join(self.resources_path, 'variables')

    def js_filepath(self, name):
        return os.path.join(self.scripts_path, name)

    def asserts_filepath(self, name):
        return os.path.join(self.asserts_path, name)

    def variables_filepath(self, name):
        return os.path.join(self.variables_path, name)

    def api_filepath_for_http_verb(self, name, verb):
        http_path = os.path.join(self.resources_path, verb)
        return os.path.join(http_path, name)

if __name__ == '__main__':
    print(ResourceProvider(r'c:\temp').js_filepath('validate.js'))
