from abc import ABCMeta, abstractstaticmethod

class ExecutorRequest(metaclass=ABCMeta):
    pass

class ApiExecutorRequest(ExecutorRequest):
    def __init__(self, apiInfo, payLoad, method):
        self.command = "api"
        self.apiInfo = apiInfo
        self.payLoad = payLoad
        self.method = method

class SetExecutorRequest(ExecutorRequest):
    def __init__(self, parameterName, parameterValue):
        self.command = "set"
        self.parameterName = parameterName
        self.parameterValue = parameterValue

class ListExecutorRequest(ExecutorRequest):
    def __init__(self):
        self.command = "list"
