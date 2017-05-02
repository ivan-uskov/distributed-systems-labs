SET ROOT_DIR=%~dp0

cd %ROOT_DIR%\frontend
call build.bat

cd ..\backend
call %ROOT_DIR%\backend\build.bat

cd ..\
                                  