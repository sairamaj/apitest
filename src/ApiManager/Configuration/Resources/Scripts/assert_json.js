// script to assert data with json object
// assert file
//      

function index(obj, i) { return obj[i] }

function assert(path, expected, obj) {
	console.log('asserting : path=' + path + '  expected=' + expected)
	try {
		actual = path.split('.').reduce(index, obj)
	}
	catch (err) {
		throw path + ' could not find'
	}

	if (actual === undefined) {
		throw "failed: path " + path + " not found"
	}
	if (expected !== actual) {
		throw "failed for " + path  + " expected " + expected + " actual:" + actual
	}
}
function after_execute(context) {
	console.log('--------- after_execute ----------')
	if (context.assert_records === undefined) {
		throw "assert_filedata_with_json script requires assert records not found."
	}
	if (context.response === undefined) {
		throw "response was not set."
	}
	console.log(context.response)
	obj = JSON.parse(context.response)
	for (key in context.assert_records) {
		console.log('verifying: ' + key)
		assert(key, context.assert_records[key], obj)
	}

	return ' validated successfully.'
}