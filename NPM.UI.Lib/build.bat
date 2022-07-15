
:: Run this batch file as: cmd /c build.bat

npm run build && npm pack && cd ..\..\catfish-form-editor && npm install ..\catfish-3.0\NPM.UI.Lib\\arcualberta-catfish-ui-1.1.0.tgz && npm run dev
