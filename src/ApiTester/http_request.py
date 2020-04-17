class HttpRequest:
    def __init__(self, apiResponse):

        # extract request
        bodyString = apiResponse.request.body
        if isinstance(bodyString, bytes):
            bodyString = apiResponse.request.body.decode("utf-8")

        if bodyString == None:
            self.request = ""
        else:
            self.request = bodyString

        # extract response
        content = apiResponse.__dict__.get('_content', None)
        if content == None:
            self.response = ""
        else:
            self.response = content
        self.status_code = apiResponse.status_code
            