from pipeserver import PipeServer
from pprint import pprint
import json

pipeServer = PipeServer('apiinfo')


def debug(response):
    pprint('_________________')
    pprint(response)
    if hasattr(response, '__dict__'):
        pprint(response.__dict__)
    pprint('________request.__dict___________')
    if hasattr(response, 'request'):
        if hasattr(response.request, '__dict__'):
            pprint(response.request.__dict__)
    pprint('_________________')


def collectlog(response, sessionName):
    debug(response)

    try:
        bodyString = response.request.body
        print(f"Type:{type(bodyString)}")

        if isinstance(bodyString, bytes):
            bodyString = response.request.body.decode("utf-8")

        pipeServer.send("api|" + json.dumps({
            "session": sessionName,
            "url": response.request.url,
            "method": response.request.method,
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
        }))
    except Exception as e:
        print('exception in collectlog. ignoring.')


def sendExtractInfo(sessionName, variable_name, value, success, message):
    data = {
        "session": sessionName,
        "variable": variable_name,
        "value": value,
        "success": success,
        "message": message}

    try:
        info = "extract|" + json.dumps(data)
        print(info)
        pipeServer.send(info)
    except:
        print('exception in sendExtractInfo. ignoring.')

def sendAssertInfo(sessionName, variable_name, expected, actual, success, message):
    data = {
        "session": sessionName,
        "variable": variable_name,
        "expected": expected,
        "actual": actual,
        "success" : success,
        "message": message}

    try:
        info = "assert|" + json.dumps(data)
        print(info)
        pipeServer.send(info)
    except:
        print('exception in sendAssertInfo. ignoring.')
