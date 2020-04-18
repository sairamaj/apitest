## single executable running gave error
* found that latest setuptools has problem
* tried __easy_install__ to go back 40.6.2 which is working
* got access denied error
* Looks like [here](https://stackoverflow.com/questions/17601020/easy-install-exe-permission-denied-on-windows-8) is problem as updating itself. copied this tool to c:\temp as suggested and ran again
* then used pip install to setuptools for 40.6.2 like "
```cmd
pip install "setuptools==40.6.2"
pip show setuptools
```