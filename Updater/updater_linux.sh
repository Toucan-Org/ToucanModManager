#!/bin/bash

# Step 1: Read the download URL from "updater_config.txt" and download the zip file
updaterConfig="updater_config.txt"
downloadUrl=$(cat "$updaterConfig")
zipFile="new_version.zip"
echo "Downloading newest version of Toucan Mod Manager..."
curl -L "$downloadUrl" -o "$zipFile"

# Step 2: Extract the zip file into the parent directory
echo Extracting zip file...
unzip -o "$zipFile" -d ..

# Step 3: Ensure InstalledMods remains intact
if [ ! -d "InstalledMods" ]; then
    echo "Error: InstalledMods folder not found. Update failed."
    exit 1
fi

# Step 4: Clean up
rm "Updater/$zipFile"
rm "Updater/$updaterConfig"

# Step 5: Start the new ToucanUI.exe and shut down the updater script
echo "Update complete. Starting Toucan Mod Manager..."
./Toucan & 
