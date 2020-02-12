from pipeserver import pipeserver
from pprint import pprint
import json

def collectlog(response):
    pprint('_________________')
    pprint(response.__dict__)
    pprint('_________________')
    pprint(response.request.__dict__)
    pprint('_________________')
    pipeserver(json.dumps({
        "url" : response.request.url,
        "method" : response.request.method,
        "statuscode" : response.reason,
        "request": { "body" : response.request.body }
    }))
