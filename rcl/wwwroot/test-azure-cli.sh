#!/bin/bash

# Echo a message to indicate the script is running
echo "Running test-azure-cli.sh"

# Check if Azure CLI is installed
if command -v az &> /dev/null
then
    echo "Azure CLI is installed"
    az --version
else
    echo "Azure CLI is not installed"
fi
