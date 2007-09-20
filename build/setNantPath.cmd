echo off
prompt $

set NANTPATH=%~p0..\tools\nant

echo.
echo -----------------------
echo Check NAnt install
echo -----------------------
if not exist "%NANTPATH%\nant-0.85\bin\nant.exe" goto nant_error

echo * NAnt is installed OK
echo.

set path=%path%;%NANTPATH%\nant-0.85\bin
goto done

:nant_error
echo * Problem: NAnt not installed
echo.
echo This build requires that NAnt is installed.  You can download NAnt v0.85 
echo from Sourceforge.net and install it to directory - 
echo   %NANTPATH%
echo.

:done
prompt