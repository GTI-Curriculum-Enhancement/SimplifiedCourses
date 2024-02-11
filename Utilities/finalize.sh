#!/bin/bash

[ ! -d "./.git" ] && {
    echo "Can not find .git directory"
    exit
}

currentDir=$(pwd)

cd "./Utilities/YamlExtractor.Net/YamlExtractor.Net"
dotnet publish -c Release -r win-x64 --no-self-contained /p:PublishSingleFile=true /p:PublishReadyToRun=true -o ../../publish

cd "../../.."
cd "./Utilities/publish" || {
    echo "Failed to cd into YamlExtractor.Net published files"
    cd "$currentDir"
    exit
}

cp "./YamlExtractor.Net.exe" ../..

# Doesn't work
#cp "./YamlExtractor.Net" ../..

cd "$currentDir"
