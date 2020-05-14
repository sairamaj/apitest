import js2py

class JsExecutor:
    def __init__(self,js_file):
        self.js_file = js_file
    
    def execute_preprocess(self, request):
        _, jsFile = js2py.run_file(self.js_file)
        context = {}
        context['request'] = request
        jsFile.before_execute(context)

    def execute(self, apiInfo):
        print(f'executing:{apiInfo.baseUrl}')
        _, jsFile = js2py.run_file(self.js_file)
        context = {}
        return jsFile.post(context)
        
    def execute_postprocess(self, request, response, script_args):
        _, jsFile = js2py.run_file(self.js_file)
        context = {}
        context['request'] = request
        context['response'] = response
        for k,v in script_args.items():
            context[k] = v
        return jsFile.after_execute(context)

    def execute_postprocess_with_asserts(self, response, assert_records):
        _, jsFile = js2py.run_file(self.js_file)
        context = {}
        context['response'] = response
        context['assert_records'] = assert_records
        return jsFile.after_execute(context)

if __name__ == '__main__':
    js = JsExecutor(r'.\resources\scripts\validate_api.js')
    js.execute_preprocess('this is request')
    js.execute_postprocess('this is request', 'this is response')

