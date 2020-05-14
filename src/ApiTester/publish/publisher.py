import json
from publish.pipeserver import PipeServer
from publish.filepublisher import FilePublisher
from pprint import pprint

pipeServer = PipeServer('apiinfo')
errorPipe = PipeServer('error')
loggerPipe = PipeServer('log')
managementPipe = PipeServer('management')


class Publisher:
    def __init__(self, properties):
        self.properties = properties

    def log(self, message):

        try:
            loggerPipe.send(message)
        except Exception as e:
            print(f'exception in log ignoring.{e}')

    def apiresult(self, response, sessionName, request_id=""):
        self.debug_api(response)
        try:
            bodyString = response.request.body

            if isinstance(bodyString, bytes):
                bodyString = response.request.body.decode("utf-8")

            data = "api|" + json.dumps({
                "session": sessionName,
                "requestid": request_id,
                "url": response.request.url,
                "method": response.request.method,
                "httpcode": response.status_code,
                "statuscode": response.reason,
                "timetaken": response.elapsed.microseconds,
                "request": {
                    "url": response.request.url,
                    "body": bodyString,
                    "headers": dict(response.request.headers)
                },
                "response": {
                    "content": response.__dict__['_content'].decode("utf-8"),
                    "headers": dict(response.headers)
                }
            })

            output_path = self.properties.get('output', None)
            if output_path != None:
                FilePublisher(output_path).api_data(data)

            pipeServer.send(data)

        except Exception as e:
            print(f'exception in collectlog. ignoring.{e}')

    def extractInfo(self, sessionName, variable_name, value, success, message):
        data = {
            "session": sessionName,
            "variable": variable_name,
            "value": value,
            "success": success,
            "message": message}

        try:
            info = "extract|" + json.dumps(data)
            print(info)

            output_path = self.properties.get('output', None)
            if output_path != None:
                FilePublisher(output_path).extract_data(info)

            pipeServer.send(info)
        except Exception as e:
            print(f'exception in extractInfo. ignoring {e}.')

    def assertInfo(self, sessionName, variable_name, expected, actual, success, message):
        data = {
            "session": sessionName,
            "variable": variable_name,
            "expected": expected,
            "actual": actual,
            "success": success,
            "message": message}

        try:
            info = "assert|" + json.dumps(data)
            print(info)

            output_path = self.properties.get('output', None)
            if output_path != None:
                FilePublisher(output_path).assert_data(info)

            pipeServer.send(info)
        except Exception as e:
            print(f'exception in assertInfo. ignoring.{e}')

    def printInfo(self, sessionName, message):
        data = {
            "session": sessionName,
            "message": message}

        try:
            info = "print|" + json.dumps(data)
            print(info)
            pipeServer.send(info)
        except Exception as e:
            print(f'exception in assertInfo. ignoring.{e}')

    def managementInfo(self, sessionName, name, info):
        data = {
            "session": sessionName,
            name: info
        }
        try:
            info = f"{name}|" + json.dumps(data)
            print(info)
            managementPipe.send(info)
        except Exception as e:
            print(f'exception in managementInfo. ignoring.{e}')

    def errorInfo(self, sessionName, error):
        data = {
            "session": sessionName,
            "error": str(error)
        }
        try:
            info = f"error|" + json.dumps(data)
            print(info)

            output_path = self.properties.get('output', None)
            if output_path != None:
                FilePublisher(output_path).error_data(info)

            pipeServer.send(info)
        except Exception as e:
            print(f'exception in errorInfo. ignoring. {str(e)}')

    def jsExecuteInfo(self, sessionName, script_file_name, message, iserror):
        data = {
            "session": sessionName,
            "scriptfilename": script_file_name,
            "message": message,
            "iserror": iserror
        }
        try:
            info = f"js|" + json.dumps(data)
            print(info)

            output_path = self.properties.get('output', None)
            if output_path != None:
                FilePublisher(output_path).js_data(info)

            pipeServer.send(info)
        except Exception as e:
            print(f'exception in jsExecuteInfo. ignoring. {str(e)}')

    def debug_api(self, response):
        pprint('_____ debug_api_begin____________')
        pprint(response)
        if hasattr(response, '__dict__'):
            pprint(response.__dict__)
        pprint('________request.__dict___________')
        if hasattr(response, 'request'):
            if hasattr(response.request, '__dict__'):
                pprint(response.request.__dict__)
        pprint('_______debug_api_end__________')
