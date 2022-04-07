@echo off
set "ori=%cd%"
cd..
cd..
set "bbd=%cd%"
set "newpath=%bbd%\HYBootLauncher.exe"
start %newpath% %1