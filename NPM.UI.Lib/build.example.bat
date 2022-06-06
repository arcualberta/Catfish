
:: Run this batch file as: cmd /c build.bat

npm run build && npm pack && cd \mypath\to\test\project && npm install \mypath\to\NPM.UI.Lib\arcualberta-catfish-ui-1.0.0.tgz && npm run serve
