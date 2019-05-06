pushd %1
call npm install
call npm run build
popd