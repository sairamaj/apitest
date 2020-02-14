from pipeserver import PipeServer
from pprint import pprint
import json

pipeServer = PipeServer()

def debug(response):
    pprint('_________________')
    pprint(response.__dict__)
    pprint('________request.__dict___________')
    pprint(response.request.__dict__)
    pprint('_________________')

def collectlog(response):
    pipeServer.send(json.dumps({
        "url" : response.request.url,
        "method" : response.request.method,
        "statuscode" : response.reason,
        "timetaken" : response.elapsed.microseconds,
        "request": { 
            "url" : response.request.url,
            "body" : response.request.body,
            "headers": dict(response.request.headers)
            },
        "response" : {
            "content": response.__dict__['_content'].decode("utf-8"),
            "headers": dict(response.headers)
            }
    }))
