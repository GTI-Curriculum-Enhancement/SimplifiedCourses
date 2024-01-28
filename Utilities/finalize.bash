#!/bin/bash

[ ! -d "./.git" ] && {
    echo "Can not find .git directory"
    exit
}

currentDir=$(pwd)

cd "./Utilities/publish" || {
    echo "Failed to cd into YamlExtractor.Net published files"
    cd "$currentDir"
    exit
}

cp "./YamlExtractor.Net.exe" ../..

# Doesn't work
#cp "./YamlExtractor.Net" ../..

cd "$currentDir"
