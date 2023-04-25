using System;
using System.IO;
using System.Diagnostics;

namespace ToucanUI.Services
{
    public class KSP2Service
    {
        public (string path, string version) DetectGameVersion(string path="")
        {
            string[] searchDirectories;

            if (string.IsNullOrEmpty(path))
            {
                // Scan common install directories
                searchDirectories = new string[] {
                Path.Combine("C:", "Program Files (x86)", "Steam", "steamapps", "common", "Kerbal Space Program 2"),
                Path.Combine("C:", "Program Files", "Steam", "steamapps", "common", "Kerbal Space Program 2"),
                Path.Combine("C:", "Program Files", "Epic Games", "Kerbal Space Program 2")
                };
            }
            else
            {
                searchDirectories = new string[] { path };
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

        private string FindKSP2Exe(string[] searchDirectories)
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
