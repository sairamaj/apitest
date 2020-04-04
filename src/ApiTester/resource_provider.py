import os

class ResourceProvider:
    def __init__(self, resources_path):
        self.resources_path = resources_path
        self.scripts_path = os.path.join(self.resources_path, 'scripts')
    
    def js_filepath(self,name):
        return os.path.join(self.scripts_path, name)

if __name__ == '__main__':
    print(ResourceProvider(r'c:\temp').js_filepath('validate.js'))