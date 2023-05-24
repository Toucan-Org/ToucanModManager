using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace ToucanUI.Services
{
    public class KSP2Service
    {
        public (string path, string version) DetectGameVersion(string path="")
        {
            List<string> searchDirectories = new List<string>();

            if (string.IsNullOrEmpty(path))
            {
                // Get all available drive letters
                DriveInfo[] allDrives = DriveInfo.GetDrives();

                foreach (DriveInfo drive in allDrives)
                {
                    // Steam
                    searchDirectories.Add(Path.Combine(drive.Name, "Program Files (x86)", "Steam", "steamapps", "common", "Kerbal Space Program 2"));
                    searchDirectories.Add(Path.Combine(drive.Name, "Program Files", "Steam", "steamapps", "common", "Kerbal Space Program 2"));
                    searchDirectories.Add(Path.Combine(drive.Name, "Steam", "steamapps", "common", "Kerbal Space Program 2"));

                    // Epic Games
                    searchDirectories.Add(Path.Combine(drive.Name, "Program Files", "Epic Games", "Kerbal Space Program 2"));
                    searchDirectories.Add(Path.Combine(drive.Name, "Epic Games", "Kerbal Space Program 2"));

                    // Generic
                    searchDirectories.Add(Path.Combine(drive.Name, "Games", "Kerbal Space Program 2"));
                }
            }
            else
            {
                searchDirectories = new List<string> { path };
            }


            string ksp2ExePath = FindKSP2Exe(searchDirectories);

            if (string.IsNullOrEmpty(ksp2ExePath))
            {
                Debug.WriteLine("Could not find KSP2_x64.exe");
                return ("", "");
            }

            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(ksp2ExePath);
            return (ksp2ExePath, versionInfo.ProductVersion);
        }

        private string FindKSP2Exe(List<string> searchDirectories)
        {
            string fileName = "KSP2_x64.exe";

            foreach (string directory in searchDirectories)
            {
                string filePath = Path.Combine(directory, fileName);

                if (File.Exists(filePath))
                {
                    Debug.WriteLine($"Found: {filePath}");
                    return filePath;
                }
            }
            return "";
        }
    }
}
