console.log('validate_api')

function before_execute(context){
   console.log('validate_api: before_execute')
   console.log(context.request)
}

function after_execute(context){
   console.log('validate_api: after_execute')
   console.log(context.request)
   console.log('______________________-')
   console.log(context.response)
}
