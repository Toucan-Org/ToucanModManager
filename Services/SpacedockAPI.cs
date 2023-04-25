using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ToucanUI.Models;
using System.Diagnostics;
using System.Text.Json;
using Avalonia.Platform;
using Avalonia;
using System.IO;

// THIS IS JUST TEMPORARY, WE CAN FIX IT UP LATER
// THE ACTUAL API IS NOT FULLY WORKING BUT DUMMY DATA DOES, CANT BE BOTHERED TO FIX IF WE ARE REWRITING THIS ANYWAY

namespace ToucanUI.Services
{
    public class SpacedockAPI
    {
        private const string BASE_URL = "https://spacedock.info/api/browse?&game_id=22407";
        private readonly HttpClient _client;

        public SpacedockAPI()
        {
            _client = new HttpClient();
        }

        public async Task<List<Mod>> GetMods(bool useDummyData = false)
        {
            var mods = new List<Mod>();


            if (useDummyData)
            {
                Debug.WriteLine("Using dummy data");
                // Load data from file
                var assembly = typeof(SpacedockAPI).Assembly;
                var resource = assembly.GetManifestResourceStream("ToucanUI.Assets.output.json");
                var jsonString = new StreamReader(resource).ReadToEnd();

                var jsonDocument = JsonDocument.Parse(jsonString);
                var data = jsonDocument.RootElement;

                mods = ParseModData(data);
            }

            else
            {
                Debug.WriteLine("Getting mod data");

                // Request mod data from the first URL
                var url = $"{BASE_URL}&page=1";
                Debug.WriteLine($"Requesting mod data from {url}");
                var response = await _client.GetAsync(url);
                Debug.WriteLine($"Response status code: {response.StatusCode}");
                var jsonString = await response.Content.ReadAsStringAsync();

                // Parse the JSON data
                var jsonDocument = JsonDocument.Parse(jsonString);
                var data = jsonDocument.RootElement;

                // Get the totalPages
                int totalPages = data.GetProperty("pages").GetInt32();

                // Now iterate through all the pages
                for (int currentPage = 1; currentPage <= totalPages; currentPage++)
                {
                    // If it's not the first page, fetch the data
                    if (currentPage != 1)
                    {
                        // Request mod data from the URL
                        url = $"{BASE_URL}&page={currentPage}";
                        Debug.WriteLine($"Requesting mod data from {url}");
                        response = await _client.GetAsync(url);
                        Debug.WriteLine($"Response status code: {response.StatusCode}");
                        jsonString = await response.Content.ReadAsStringAsync();

                        // Parse the JSON data
                        jsonDocument = JsonDocument.Parse(jsonString);
                        data = jsonDocument.RootElement;
                    }

                    // Parse the mod data
                    var pageMods = ParseModData(data);
                    mods.AddRange(pageMods);

                    Debug.WriteLine($"Finished retrieving {pageMods.Count} mods from page {currentPage}");
                }
            }


            return mods;

        }

        public List<Mod> ParseModData(JsonElement data)
        {
            var mods = new List<Mod>();

            foreach (var item in data.GetProperty("result").EnumerateArray())
            {
                try
                {
                    var mod = new Mod(item);
                    mods.Add(mod);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error: {ex}");
                }

                Debug.WriteLine($"Retrieved {mods.Count} mods so far");
            }

            return mods;
        }

    }
}
