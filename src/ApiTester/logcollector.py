from pipeserver import PipeServer
from pprint import pprint
import json

pipeServer = PipeServer()

def collectlog(response):
    pprint('_________________')
    pprint(response.__dict__)
    pprint('_________________')
    pprint(response.request.__dict__)
    pprint('_________________')
    pipeServer.send(json.dumps({
        "url" : response.request.url,
        "method" : response.request.method,
        "statuscode" : response.reason,
        "request": { "body" : response.request.body }
    }))
