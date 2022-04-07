@echo off
set "ori=%cd%"
cd..
set "bbd=%cd%"
set "newpath=%bbd%\dcl.clientapply.exe"
start %newpath% %1