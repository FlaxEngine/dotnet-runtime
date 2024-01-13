@echo off
setlocal

set _args=%*
if "%~1"=="-?" set _args=-help
if "%~1"=="/?" set _args=-help

:: Ensure to have proper version files ready before build
call "%~dp0eng\native\version\copy_version_files.cmd"

powershell -ExecutionPolicy ByPass -NoProfile -Command "& '%~dp0eng\build.ps1'" %_args%
exit /b %ERRORLEVEL%
