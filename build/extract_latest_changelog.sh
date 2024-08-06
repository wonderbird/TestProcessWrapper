#!/usr/bin/env bash
#
# Extract the introduction and the entry for the latest version from CHANGELOG.md
#
# WARNING: Duplicate Code
#
# This script is maintained in two different GitHub repositories. The "copy" is in
# https://github.com/wonderbird/malimo/blob/main/build/extract_latest_changelog.sh
#
# Refactor as soon as a third copy is created.
#
set -euxf

# The if construct around the read command suppresses the error returned by read.
# read returns 1, because it does not find the delimiter '' in the input.
# See also: https://stackoverflow.com/questions/40547032/bash-read-returns-with-exit-code-1-even-though-it-runs-as-expected#67066283
if read -rd '' extract_intro_and_latest_release; then :; fi << 'EOF'
BEGIN {
  number_of_parsed_version_headings = 0
}
{
  # count the number of version headings
  if ($0 ~ /^## \[[Uu]nreleased\]/) {  } # do not count the "Unreleased" section
  else if ($0 ~ /^## /) { number_of_parsed_version_headings++ }
  
  # print the top of the changelog until end of the recently released version
  if (number_of_parsed_version_headings < 2) { print }
}
EOF

# 1. extract only the intro + latest release from CHANGELOG.md (= the awk program)
# 2. remove empty lines from end of output (= the sed command)
#    (see https://stackoverflow.com/questions/7359527/removing-trailing-starting-newlines-with-sed-awk-tr-and-friends)
awk "$extract_intro_and_latest_release" ./CHANGELOG.md | sed -e :a -e '/^\n*$/{$d;N;};/\n$/ba'
