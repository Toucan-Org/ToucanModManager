using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// The Mod Object, which is used to store the data from the Spacedock API. This can be improved greatly

namespace ToucanUI.Models
{
    public class Mod : ReactiveObject
    {

        // =====================
        // MOD VARIABLES
        // =====================

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



        // =====================
        // VERSION VARIABLES
        // =====================

        // List of all versions
        public List<Version> Versions { get; set; }

        // Latest version (keeping these to be used for easy comparison if mod is up to date or not)
        public Version LatestVersion;

        // Currently selected version 
        private Version _selectedVersion;
        public Version SelectedVersion
        {
            get => _selectedVersion;
            set => this.RaiseAndSetIfChanged(ref _selectedVersion, value);
        }


        // =====================
        // MOD CONSTRUCTOR
        // =====================
        public Mod(JsonElement modJson)
        {
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
            License = modJson.GetProperty("license").GetString();
            Website = modJson.GetProperty("website").GetString();
            Donations = modJson.GetProperty("donations").GetString();
            SourceCode = modJson.GetProperty("source_code").GetString();
            Url = modJson.GetProperty("url").GetString();
            Versions = new List<Version>();

            // Create version objects for each version
            var versionJsonArray = modJson.GetProperty("versions").EnumerateArray();
            foreach (var versionJson in versionJsonArray)
            {
                var versionID = versionJson.GetProperty("id").GetInt32();
                var version = new Version(versionJson, this)
                {
                    VersionID = versionID,
                    FriendlyVersion = versionJson.GetProperty("friendly_version").GetString(),
                    GameVersion = versionJson.GetProperty("game_version").GetString(),
                    Changelog = versionJson.GetProperty("changelog").GetString(),
                    DownloadPath = "https://spacedock.info" + versionJson.GetProperty("download_path").GetString(),
                    Downloads = versionJson.GetProperty("downloads").GetInt32(),
                    Created = versionJson.GetProperty("created").GetDateTimeOffset(),
                    IsSelectedVersion = false
                };

                // Add version to list of versions
                Versions.Add(version);
            }

            // Get the latest version and set it as default
            LatestVersion = GetLatestVersion();
            SelectedVersion = LatestVersion;
            SelectedVersion.IsSelectedVersion = true;

            // Only grab the download size for the latest version (speeds up loading)
            Task.Run(() => LatestVersion.GetDownloadSizeAsync());

        }

        // ===============
        // METHODS
        // ===============

        // Method to get the latest version
        private Version GetLatestVersion()
        {
            return Versions.OrderByDescending(v => v.Created).FirstOrDefault();
        }

        // Method to select only one version
        public void OnVersionSelected(Version selectedVersion)
        {

            // Deselect other versions
            foreach (var version in Versions)
            {
                if (version != selectedVersion)
                {
                    version.IsSelectedVersion = false;
                }
            }

            // Update the SelectedVersion property
            SelectedVersion = selectedVersion;
            SelectedVersion.IsSelectedVersion = true;
        }


        // Method to get the download size of all versions of a mod
        public async Task InitializeDownloadSizesAsync()
        {
            var downloadTasks = Versions.Select(version => version.GetDownloadSizeAsync());
            await Task.WhenAll(downloadTasks);

        }

    }

}
