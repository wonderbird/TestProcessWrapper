#!/bin/sh
# Identify code duplication
#
# Prerequisite:
# - Install the JetBrains ReSharper Command Line Tools via dotnet tool:
#   https://www.jetbrains.com/help/resharper/ReSharper_Command_Line_Tools.html
# - Have xsltproc installed (most probably preinstalled on your OS)
#   http://xmlsoft.org/XSLT/xsltproc2.html
#
# Usage:
# tools\dupfinder.sh
#
# A report showing duplicate code parts will be written to the file
# "dupfinder-report.xml" in the current directory.
#
# Auto-generated files are ignored using the --exclude flag with a
# pattern.
#
# The sensitivity regarding when to consider a code fragment a
# duplicate is controlled via the --discard-cost parameter.
#
# The command requires the directory c:\temp to exist. There, the
# cache folder dupfinder-cache will be created temporarily.
#
# For detailed information see ReSharper Command Line Tools:
# https://www.jetbrains.com/help/resharper/dupFinder.html
#

PROJECT_NAME=RemoteControlledProcess
jb dupfinder --exclude="*\obj\**;*\Features\*.feature.cs" --discard-cost=50 --discard-literals=true --show-text --o=dupfinder-report.xml "$PROJECT_NAME.sln"
xsltproc "tools/dupfinder.xslt" "dupfinder-report.xml" > "dupfinder-report.html" 
