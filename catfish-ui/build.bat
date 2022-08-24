
:: Run this batch file as: cmd /c build.bat

vite build && xcopy dist\*.js ..\Catfish\Catfish.Web\wwwroot\assets\applets\ /Y && xcopy dist\*.css ..\Catfish\Catfish.Web\wwwroot\assets\applets\ /Y

