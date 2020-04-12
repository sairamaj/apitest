##############################
# File format
#   url
#   headers_begin
#   headers_end
#   request_begin
#   request_end
#   sample:
#https://login.apigee.com/oauth/token
#
# headers_begin
# Authorization:Basic ZWRnZWNsaTplZGdlY2xpc2VjcmV0
# headers_end
#
# request_begin
# grant_type=password&username=sairamaj%40hotmail.com&password=ped~
# request_end
##############################

import sys
from utils import readAllText

class HttpRequestData:
    def __init__(self, file_name):
        self.headers = {}
        self.request = ""
        self.url = ""
        self.method = "get"
        self._parse(file_name)

    def _parse(self, file_name):
        with open(file_name, 'r') as file:
            self.method = file.readline().rstrip('\n').lower()
            self.url = file.readline().rstrip('\n')
            headers_started = False
            request_started = False
            for line in file.readlines():
                line = line.strip(' ')
                if line.startswith('headers_begin'):
                    headers_started = True
                elif line.startswith('headers_end'):
                    headers_started = False
                elif headers_started == True:
                    line = line.strip(' ').rstrip('\n')
                    if len(line) > 0:
                        parts = line.split(':')
                        header_name = parts[0]
                        header_value = ""
                        if len(parts) > 1:
                            header_value = parts[1]
                        self.headers[header_name] = header_value
                if line.startswith('request_begin'):
                    request_started = True
                elif line.startswith('request_end'):
                    request_started = False
                elif request_started == True:
                    self.request += line

if __name__ == "__main__":
    req_data = HttpRequestData(sys.argv[1])
    print(f"|{req_data.method}|")
    print(f"|{req_data.url}|")
    print('_________ headers ______________')
    for k, v in req_data.headers.items():
        print(f"{k}:{v}")
    print('_________ headers ______________')
    print(req_data.request)    

