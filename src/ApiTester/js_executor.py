import js2py

class JsExecutor:
    def __init__(self,js_file):
        self.js_file = js_file
    
    def execute_preprocess(self, request):
        eval_res, jsFile = js2py.run_file(self.js_file)
        context = {}
        context['request'] = request
        jsFile.before_execute(context)

    def execute_postprocess(self, request, response):
        eval_res, jsFile = js2py.run_file(self.js_file)
        context = {}
        context['request'] = request
        context['response'] = response
        jsFile.after_execute(context)


if __name__ == '__main__':
    js = JsExecutor(r'.\resources\scripts\validate_api.js')
    js.execute_preprocess('this is request')
    js.execute_postprocess('this is request', 'this is response')

