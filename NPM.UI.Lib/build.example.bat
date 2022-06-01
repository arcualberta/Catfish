
:: Run this batch file as: cmd /c build.bat

npm run build && npm pack && cd ..\..\ig-directory && npm install ..\catfish-2.0\NPM.UI.Lib\arcualberta-catfish-ui-1.0.0.tgz && npm run serve
