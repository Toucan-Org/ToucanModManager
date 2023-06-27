#!/bin/bash

# Step 1: Read the download URL from "updater_config.txt" and download the zip file
updaterConfig="updater_config.txt"
downloadUrl=$(cat "$updaterConfig")
zipFile="new_version.zip"
echo "Downloading newest version of Toucan Mod Manager..."
curl -o "$zipFile" "$downloadUrl"

# Step 2: Extract the zip file into the parent directory
echo "Extracting zip file..."
unzip -o "Updater/$zipFile" -d ..

# Step 3: Clean up
rm "Updater/$zipFile"
rm "Updater/$updaterConfig"

# Step 4: Start the new Toucan.app and shut down the updater script
echo "Update complete. Starting Toucan Mod Manager..."
open ../Toucan
