#!/usr/bin/env bash
#
# Extract the introduction and the entry for the latest version from CHANGELOG.md
#
read -rd '' extract_intro_and_latest_release << 'EOF'
BEGIN {
  number_of_parsed_version_headings = 0
}
{
  if ($0 ~ /^## /) { number_of_parsed_version_headings++ }
  
  if (number_of_parsed_version_headings < 2) { print }
}
EOF

# 1. extract only the intro + latest release from CHANGELOG.md
# 2. remove empty lines from end of output
#    (see https://stackoverflow.com/questions/7359527/removing-trailing-starting-newlines-with-sed-awk-tr-and-friends)
awk "$extract_intro_and_latest_release" ../CHANGELOG.md | sed -e :a -e '/^\n*$/{$d;N;};/\n$/ba'
