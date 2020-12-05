@echo off

REM Identify code duplication
REM
REM Usage:
REM tools\dupfinder.bat
REM
REM A report showing duplicate code parts will be written to the file
REM "dupfinder-report.xml" in the current directory.
REM
REM Auto-generated files are ignored using the --exclude flag with a
REM pattern.
REM
REM The sensitivity regarding when to consider a code fragment a
REM duplicate is controlled via the --discard-cost parameter.
REM
REM The command requires the directory c:\temp to exist. There, the
REM cache folder dupfinder-cache will be created temporarily.
REM
REM For detailed information see ReSharper Command Line Tools:
REM https://www.jetbrains.com/help/resharper/dupFinder.html
REM

set PROJECT_NAME=kata-gof-pattern-facade-windforecast
dupfinder.exe --exclude="%PROJECT_NAME%\obj\**" --discard-cost=50 --discard-literals=true --show-text --o=dupfinder-report.xml --caches-home=c:\temp\dupfinder-cache "%PROJECT_NAME%.sln"
tools\msxsl.exe "dupfinder-report.xml" "tools\dupfinder.xslt" -o "dupfinder-report.html"
