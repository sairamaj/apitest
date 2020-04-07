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
