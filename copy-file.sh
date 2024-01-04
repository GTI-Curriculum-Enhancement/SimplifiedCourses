#!/bin/bash

###
# Copy the .YAML file content into a code block for README.md
###

if [ -f "A+ Course.yaml" ]; then
    cat "A+ Course.yaml" > "README.md"
    echo "A+ Course.yaml forwarded into README.md"
else
   echo "Failed to locate 'A+ Course.yaml'"
fi
