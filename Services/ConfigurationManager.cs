using IniParser;
using IniParser.Model;
using System;
using System.Diagnostics;
using System.IO;

namespace ToucanUI.Services
{
    public class ConfigurationManager
    {
        // =====================
        // VARIABLES
        // =====================

        // Used to store the path to the config file
        private readonly string _configFilePath;



        // =====================
        // CONSTRUCTOR
        // =====================
        public ConfigurationManager(string configFileName = "config.ini")
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string dataFolderPath = Path.Combine(exePath, "data");

            if (!Directory.Exists(dataFolderPath))
            {
                try
                {
                    Directory.CreateDirectory(dataFolderPath);
                }
                catch
                {
                    Debug.WriteLine($"Error creating directory for {configFileName}!");
                }

            }

            // Set the config file path
            _configFilePath = Path.Combine(dataFolderPath, configFileName);
        }


        // =====================
        // METHODS
        // =====================

        // Used to retrieve Time Played
        public int GetTimePlayed()
        {
            var parser = new FileIniDataParser();

            if (!File.Exists(_configFilePath))
            {
                return 0;
            }

            // Load the data from the config file
            IniData data = parser.ReadFile(_configFilePath);

            // Read the time played value from the config
            string timePlayedStr = data["Game"]["TimePlayed"];
            if (!int.TryParse(timePlayedStr, out int timePlayedSeconds))
            {
                timePlayedSeconds = 0;
            }

            return timePlayedSeconds;
        }

        // Used to store Time Played in the config
        public void SetTimePlayed(int timePlayedSeconds)
        {
            var parser = new FileIniDataParser();

            // Load the data from the config file
            IniData data = parser.ReadFile(_configFilePath);

            // Read the existing time played value from the config
            int existingTimePlayed;
            if (!int.TryParse(data["Game"]["TimePlayed"], out existingTimePlayed))
            {
                // Set a default value (e.g., 0) if the parsing fails
                existingTimePlayed = 0;
            }

            // Add the new time played value to the existing value
            int updatedTimePlayed = existingTimePlayed + timePlayedSeconds;

            // Update the time played value in the config
            data["Game"]["TimePlayed"] = updatedTimePlayed.ToString();

            // Save the updated config back to the file
            parser.WriteFile(_configFilePath, data);
        }


        // Used to save current settings to Config
        public void SaveConfig(string gamePath, string gameVersion)
        {
            // Create an instance of the INI file parser
            var parser = new FileIniDataParser();

            // Check if the config file exists, create one if it doesn't
            if (!File.Exists(_configFilePath))
            {
                File.Create(_configFilePath).Dispose();
            }

            // Load the data from the config file
            IniData data = parser.ReadFile(_configFilePath);

            // Add or update the game path and version in the config
            data["Game"]["Path"] = gamePath;
            data["Game"]["Version"] = gameVersion;

            // Save the updated config back to the file
            parser.WriteFile(_configFilePath, data);
        }

        // Used to retrieve Game Path
        public string GetGamePath()
        {
            // Create an instance of the INI file parser
            var parser = new FileIniDataParser();

            // Check if the config file exists, return null if it doesn't
            if (!File.Exists(_configFilePath))
            {
                return null;
            }

            // Load the data from the config file
            IniData data = parser.ReadFile(_configFilePath);

            // Return the game path from the config, or null if it doesn't exist
            return data["Game"]["Path"] ?? null;
        }

        // Used to retrieve Game Version
        public string GetGameVersion()
        {
            var parser = new FileIniDataParser();

            if (!File.Exists(_configFilePath))
            {
                return null;
            }

            // Load the data from the config file
            IniData data = parser.ReadFile(_configFilePath);

            // Return the game path from the config, or null if it doesn't exist
            return data["Game"]["Version"] ?? null;

        }

        // Used to set the game path in the config
        public void SetGamePath(string gamePath)
        {
            var parser = new FileIniDataParser();

            if (!File.Exists(_configFilePath))
            {
                return;
            }

            // Load the data from the config file
            IniData data = parser.ReadFile(_configFilePath);

            // Update the game path in the config
            data["Game"]["Path"] = gamePath;

            // Save the updated config back to the file
            parser.WriteFile(_configFilePath, data);
        }

        // Used to set the game version in the config
        public void SetGameVersion(string gameVersion)
        {
            var parser = new FileIniDataParser();

            if (!File.Exists(_configFilePath))
            {
                return;
            }

            // Load the data from the config file
            IniData data = parser.ReadFile(_configFilePath);

            // Update the game version in the config
            data["Game"]["Version"] = gameVersion;

            // Save the updated config back to the file
            parser.WriteFile(_configFilePath, data);
        }


        // Clear the config file
        public void ClearConfig()
        {
            // Create an instance of the INI file parser
            var parser = new FileIniDataParser();
            // Check if the config file exists, return null if it doesn't
            if (!File.Exists(_configFilePath))
            {
                return;
            }
            // Load the data from the config file
            IniData data = parser.ReadFile(_configFilePath);
            // Clear the game path and version from the config
            data["Game"]["Path"] = "";
            data["Game"]["Version"] = "";
            data["Game"]["TimePlayed"] = "";
            // Save the updated config back to the file
            parser.WriteFile(_configFilePath, data);
        }
    }

}
