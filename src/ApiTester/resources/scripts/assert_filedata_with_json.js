// script to assert data with json object
// assert file
//      

const fs = require('fs')

function after_execute(context){
    if (context.assert_file === undefined){
        throw "assert_filedata_with_json script requires assert file name in the context."
    }

    var assrt_data = fs.readFileSync(context.assert_file,'utf-8')
    console.log(assert_data)
    assert_file = context.assert_file
    return assert_file + ' validated successfully.'
}