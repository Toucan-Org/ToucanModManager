using System;
using System.IO;
using IniParser;
using IniParser.Model;

namespace ToucanUI.Services
{
    public class ConfigurationManager
    {
        private readonly string _configFilePath;

        public ConfigurationManager(string configFileName = "config.ini")
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string dataFolderPath = Path.Combine(exePath, "data");

            if (!Directory.Exists(dataFolderPath))
            {
                Directory.CreateDirectory(dataFolderPath);
            }

            _configFilePath = Path.Combine(dataFolderPath, configFileName);
        }

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
            // Save the updated config back to the file
            parser.WriteFile(_configFilePath, data);
        }
    }

}
