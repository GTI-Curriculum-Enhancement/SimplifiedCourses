#!/bin/bash

echo "This is no longer in use as YamlExtractor.Net is more efficient"
exit 1

###
# Copy the .YAML file content into a code block for README.md
###

if [ -f "A+ Course.yaml" ]; then
    echo -ne '```yaml\n' > "README.md"  # Clear file and add start of code block
    cat 'A+ Course.yaml' >> "README.md"   # Move YAML data
    echo -ne '```\n' >> "README.md"     # Finalize code block
    echo "A+ Course.yaml forwarded into README.md"
else
   echo "Failed to locate 'A+ Course.yaml'"
fi
