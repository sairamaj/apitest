class ApiException(Exception):
    def __init__(self,code, error):
        self.code = code
        self.error = error
    
    def __str__(self):
        return f"{self.code}:{self.error}"
