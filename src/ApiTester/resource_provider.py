import os

class ResourceProvider:
    def __init__(self,workingDirectory):
        self.workingDirectory = workingDirectory
        self.resources_path = os.path.join(self.workingDirectory, 'resources')
        self.scripts_path = os.path.join(self.resources_path, 'scripts')
    
    def js_filepath(self,name):
        return os.path.join(self.scripts_path, name)

if __name__ == '__main__':
    print(ResourceProvider(r'c:\temp').js_filepath('validate.js'))