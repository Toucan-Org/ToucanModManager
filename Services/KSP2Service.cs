using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace ToucanUI.Services
{
    public class KSP2Service
    {
        // =====================
        // METHODS
        // =====================

        // Method to detect current game version and path for KSP2
        public (string path, string version) DetectGameVersion(string path = "")
        {
            List<string> searchDirectories = new List<string>();

            if (string.IsNullOrEmpty(path))
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Windows search directories
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
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    // macOS search directories
                    searchDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library", "Application Support", "Steam", "steamapps", "common", "Kerbal Space Program 2"));
                    searchDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library", "Application Support", "Epic", "Kerbal Space Program 2"));
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    // Linux search directories
                    searchDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".local", "share", "Steam", "steamapps", "common", "Kerbal Space Program 2"));
                    searchDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".local", "share", "Epic", "Kerbal Space Program 2"));
                }
            }
            else
            {
                searchDirectories = new List<string> { path };
            }

            string ksp2ExePath = FindKSP2Exe(searchDirectories);

            if (string.IsNullOrEmpty(ksp2ExePath))
            {
                Debug.WriteLine("Could not find KSP2 executable");
                return ("", "");
            }

            string version;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                version = GetFileVersionMacOS(ksp2ExePath);
            }
            else
            {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(ksp2ExePath);
                version = versionInfo.ProductVersion;
            }


            return (ksp2ExePath, version);
        }

        // Method to find KSP2 executable
        private string FindKSP2Exe(List<string> searchDirectories)
        {
            string fileName;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                fileName = "KSP2_x64.exe";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                fileName = "KSP2";
            }
            else
            {
                throw new NotSupportedException("Unsupported platform");
            }

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

        private string GetFileVersionMacOS(string filePath)
        {
            try
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "mdls",
                        Arguments = $"-name kMDItemVersion \"{filePath}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                string version = output.Split('=').LastOrDefault()?.Trim();
                return version.Trim('"');
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex}");
                return "";
            }
        }

    }
}
