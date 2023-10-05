
:: BEGIN: Set up
:: =============
:: Initializing developer-setup specific parameters
set APP_PATH=C:\Projects\Catfish-3.0\catfish-ui
set LIB_PATH=C:\Projects\arc-foundation\arc-foundation
set LIB_NAME=@arc/arc-foundation

:: END: Set up
:: ===========

echo APP_PATH = %APP_PATH%
echo LIB_PATH = %LIB_PATH%

:: Getting the node package version string and settingit to the VERSION variable 
cd %LIB_PATH%
FOR /F "tokens=*" %%g IN ('node -p "require('./package.json').version"') do (SET VERSION=%%g)
cd %APP_PATH%
echo Found version %VERSION%

:: Calculating the lib file name by removing @ characters
:: and replacing "/" with "-" characters, and trimming off any
:: unintentional white spaces, and finally appending the VERSION 
:: and the ".tgz" extension
set LIB_FILE_NAME=%LIB_NAME:@=%
set LIB_FILE_NAME=%LIB_FILE_NAME:/=-%
set LIB_FILE_NAME=%LIB_FILE_NAME: =%
set LIB_FILE_NAME=%LIB_FILE_NAME%-%VERSION%.tgz

set LIB_FILE_FULL_PATH=%LIB_PATH%\%LIB_FILE_NAME%
echo Lib File Path Name: %LIB_FILE_FULL_PATH%

:: Changing to the lib directory, compiling and packing, moving back to the
:: app directory, and installing the library inside one child shell such that
:: if any of the commands fails, the control will return to the current directory.6
cmd /c "cd %LIB_PATH% && npm run build && npm pack && cd %APP_PATH% && npm install %LIB_FILE_FULL_PATH%"
