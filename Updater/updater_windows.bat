@echo off

:: Step 1: Read the download URL from "updater_config.txt" and download the zip file
set "updaterConfig=updater_config.txt"
set /p downloadUrl=<"%updaterConfig%"
set "zipFile=new_version.zip"
echo Downloading newest version of Toucan Mod Manager...
powershell -Command "(New-Object System.Net.WebClient).DownloadFile('%downloadUrl%', '%zipFile%')"

:: Step 2: Extract the zip file into the parent directory
cd ..
echo Extracting zip file...
powershell -Command "Expand-Archive -Path 'Updater\%zipFile%' -DestinationPath '.'"


:: Step 3: Ensure InstalledMods remains intact
if not exist "InstalledMods" (
    echo Warning: InstalledMods folder not found.
)

:: Step 4: Clean up
del "Updater\%zipFile%"
del "Updater\%updaterConfig%"


:: Step 4: Start the new ToucanUI.exe and shut down the updater script
echo Update complete. Starting Toucan Mod Manager...
start Toucan.exe
