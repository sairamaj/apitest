## API Tester

### What it is?
To test REST APIs with validations

### Concept
Every RestAPI has __request__ consists of __url__  with optional query string, __headers__ (optional) and __body__(optional) and sent to __host__ to get a __response__ with __headers__. 

A __request__ (url, headers, body) is given a short __command__

All info (url,headers,body) are __parameterized__ with __variables__

__variables__ are extracted from __response__ which can be used in next requests
 
 using __jsonpaths__  __assertions__  are made with expected values to validate response.

__information__ is shown in GUI window.

### Example

__request__
```json
{
	"accesstoken": {
		"baseurl": "https://login.apigee.com/oauth/token",
		"password": {
			"path": "",
			"body": {
				"grant_type": "password",
				"username": "{{username}}",
				"password": "{{password}}"
			},
			"headers": {
				"Authorization": "Basic ZWRnZWNsaTplZGdlY2xpc2VjcmV0"
			}
		}
    },
    	"apis": {
		"baseurl": "https://api.enterprise.apigee.com/v1/organizations/{{org_name}}",
		"_": {
			"path": "/apis"
		}
	},
}
```
__available commands__ 
```cmd
 accesstoken.password
 apis
 ```
__available variables__
```cmd
username
password
org_name
```
Now these commands can be executed in __console__ as __repl__ commands or grouped in to a __scenario__ and runas group commands

### Features
* apis mapped to simple commands
* commands can be executed in console in REPL mode.
* responses viewed in GUI tool
* scenarios can be created and run as group.

### Architecture
* A __console__ version tool written in __python__ to submit 
requests and responses
* A __GUI__ version tool writtein in __C#__ to view the data
* Using __named pipes__ to communicate between the 2 tools

### Screenshots
#### GUI
  ![apimanager](doc/images/apimanager.png)
  ![apitester](doc/images/apitester.png)

### Terminology
|  definition   |                       description                                     |
|---------------|-----------------------------------------------------------------------|
|  __api__      |  Represented by JSON file contains all api routes,headers, paths      |
| __apicommand__|  Short command representing all the details of particular route       |
| __command__   |  Console Management command (starts with !)                           |
| __extract__   |  Extraction to a variable with json path from last response           |
| __assert__    |  Asserting a value from response with expected value                  |
| __script__    |  JavaScript to process the response                                   | 
| __resource__  |  Assert,Script, Post data files in resource directory                 |

Full details of terms can be viewed [here](.\doc\terminology_details.MD)

### Structure
```cmd
\Apis
   \Apigee
        config.json
        \scenarios
            create-developer.txt
            list_api.txt
            list_deployments.txt
        \environments
            sairamaj_account.var
   \Azure
        config.json
        \scenarios
            get_accesstoken.txt
        \enviornments
            sairam_visualstudio_accunt.var
   \Custom
        ...
\Resources
   \Asserts
   \Scripts
   \Post
   \Patch
   \Delete
```
## Avaialble commands

  |    Command              |                Description                                              |                     
  | ------------            | ------------------------------------------------------------------------------|
  | Api commands            | Dynamically populated from config.json. See the API command sections          |
  | __!assert__             | Asserts a value from response using JSON path with expected value             |
  | __!assert_with_js__     | Asserts a with values from file against response using JSON path              |
  | __!extract__            | Extracts from last response using JSON Path in to a variable                  |
  | __!help__               | Displays the help                                                             |
  | __!convert_json_html__  | Converts a JSON in to HTML                                                    |
  | __!set__                | Sets a variable value in the session properties                               |
  | __!list__               | Lists all existing variables in current session                               |
  | __!management__         | Management commands                                                           |
  |                         |      __apicommands__ Api commands for given API configuration                 |
  |                         |      __variables__   Extracts defined variables in current API configuraiton  |
  |                         |      __commands__    Gets all available commands                              |
  | __!return__             | Useful in batch commands to stop executing at this point                      |
  | __!waitforuserinput__   | Waits for user input before proceeding to next command                        |