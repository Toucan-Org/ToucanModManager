using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace ToucanUI.Models
{
    public class Mod : INotifyPropertyChanged
    {
        private bool _isSelected;
        public string Name { get; set; }
        public int Id { get; set; }
        public string Game { get; set; }
        public int GameId { get; set; }
        public string Description { get; set; }
        public int Downloads { get; set; }
        public int Followers { get; set; }
        public string Author { get; set; }
        public int DefaultVersionId { get; set; }
        public List<string> SharedAuthors { get; set; }
        public string Background { get; set; }
        public int BgOffsetY { get; set; }
        public string License { get; set; }
        public string Website { get; set; }
        public string Donations { get; set; }
        public string SourceCode { get; set; }
        public string Url { get; set; }
        public List<Version> Versions { get; set; }
        public object ModVersion { get; private set; }
        public object GameVersion { get; private set; }

        public bool IsSelected { get; set; }
        public bool IsInstalled { get; set; }

        //THESE ARE PLACEHOLDERS FOR THE UI
        private int _progress;
        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Mod(JsonElement modJson)
        {
            IsSelected = false;
            IsInstalled = false;
            Progress = 0;
            Name = modJson.GetProperty("name").GetString();
            Id = modJson.GetProperty("id").GetInt32();
            Game = modJson.GetProperty("game").GetString();
            GameId = modJson.GetProperty("game_id").GetInt32();
            Description = modJson.GetProperty("short_description").GetString();
            Downloads = modJson.GetProperty("downloads").GetInt32();
            Followers = modJson.GetProperty("followers").GetInt32();
            Author = modJson.GetProperty("author").GetString();
            DefaultVersionId = modJson.GetProperty("default_version_id").GetInt32();
            SharedAuthors = JsonSerializer.Deserialize<List<string>>(modJson.GetProperty("shared_authors").ToString());
            Background = modJson.GetProperty("background").GetString();
            BgOffsetY = modJson.GetProperty("bg_offset_y").GetInt32();
            License = modJson.GetProperty("license").GetString();
            Website = modJson.GetProperty("website").GetString();
            Donations = modJson.GetProperty("donations").GetString();
            SourceCode = modJson.GetProperty("source_code").GetString();
            Url = modJson.GetProperty("url").GetString();
            Versions = new List<Version>();

            var versionJsonArray = modJson.GetProperty("versions").EnumerateArray();
            foreach (var versionJson in versionJsonArray)
            {
                var version = new Version(versionJson)
                {
                    FriendlyVersion = versionJson.GetProperty("friendly_version").GetString(),
                    GameVersion = versionJson.GetProperty("game_version").GetString(),
                    DownloadPath = versionJson.GetProperty("download_path").GetString(),
                    Changelog = versionJson.GetProperty("changelog").GetString(),
                    Downloads = versionJson.GetProperty("downloads").GetInt32(),
                    Created = versionJson.GetProperty("created").GetDateTimeOffset()
                };
                Versions.Add(version);
            }

            // Get the latest version
            var latestVersion = Versions.OrderByDescending(v => v.Created).FirstOrDefault();
            if (latestVersion != null)
            {
                ModVersion = latestVersion.FriendlyVersion;
                GameVersion = latestVersion.GameVersion;
            }
        }

    }

    public class Version
    {
        public string VersionNumber { get; set; }
        public string GameVersion { get; set; }
        public JsonElement VersionJson { get; }
        public string? FriendlyVersion { get; internal set; }
        public string? DownloadPath { get; internal set; }
        public string? Changelog { get; internal set; }
        public int Downloads { get; internal set; }
        public DateTimeOffset Created { get; internal set; }

        public Version(JsonElement versionJson)
        {
            VersionJson = versionJson;
        }
    }
}
