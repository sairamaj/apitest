## API Tester

### What it is?
To test REST APIs with validations

### Concept
Every RestAPI has __request__ consists of __url__  with optional query string __headers__ (optional) and __body__(optional) and sent to __host__ and a __response__ is obtained. 

A __request__ (url, headers, body) is given a short __command__

All info (url,headers,body) are __parameterized__ with __variables__

__variables__ are extracted from __response__ and __asserted__ with expected values.

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
* __api__ 
    * api information where all routes are mapped to simple commands
        * example( apigee)
```json
                "accesstoken": {
                    "baseurl": "https://login.microsoftonline.com/{{tenant_id}}",
                    "_": {
                        "path": "/oauth2/token",
                        "body": {
                            "grant_type": "client_credentials",
                            "client_id": "{{client_id}}",
                            "client_secret": "{{client_secret}}"
                        }
                    }
                }
            }
```
* __scenario__
    * group of commands where each command correspond to api call
        * example (list_developers.txt)
            * \# this file creates acces token and executes apis and gets list of developers
            * accesstoken.password
            * apis
            * developers          
* __environment__
    * list of variables which defines the environment
        * example (sairamaj_accunt.var)
            * org_name=sairamaj-eval
            * username=sairamaj@hotmail.com
            * password=pwdhere
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
```
## Avaialble commands

  |    Command              |                Description                                              |                     
  | ------------            | ------------------------------------------------------------------------------|
  | Api commands            | Dynamically populated from config.json. See the API command sections          |
  | __!assert__             | Checks expected and actual variable value matches                             |
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