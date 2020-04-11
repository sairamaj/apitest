from http_requestdata import HttpRequestData
from http_request2 import HttpRequest2

req_data = HttpRequestData(r"c:\temp\req.txt")
print(f"|{req_data.method}|")
print(f"|{req_data.url}|")
print('_________ headers ______________')
for k, v in req_data.headers.items():
    print(f"{k}:{v}")
print('_________ headers ______________')
print(req_data.request)    

request2 = HttpRequest2(req_data.url,req_data.method,req_data.headers,req_data.request)
response = request2.post()
print(response)
print(response.json())