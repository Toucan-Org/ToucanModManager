﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToucanServices.SpacedockAPI.Models;

// CURRENTLY ACCEPTS ONLY MOD

namespace ToucanServices.SpacedockAPI
{
    public static class SpacedockAPIRetrieveModSearch
    {
        /// <summary>
        /// Gets the api url for searching based off of two inputs, the search query which 
        /// defines what to search for, and the search type which selects from which category 
        /// to search in.
        /// </summary>
        /// <param name="SearchType"></param>
        /// <param name="SearchQuery"></param>
        /// <returns></returns>
        public static string GetSeachModApiUrl(string SearchQuery) 
            => $"https://spacedock.info/api/search/mod?query={SearchQuery}";

        /// <summary>
        /// Retrieve the Json contents in a string from the api url generated by the 
        /// get search api url function.
        /// </summary>
        /// <param name="SearchType"></param>
        /// <param name="SearchQuery"></param>
        /// <returns></returns>
        public static async Task<string> GetSearchModJsonString(string SearchQuery)
        {
            string SearchUrl = GetSeachModApiUrl(SearchQuery);

            HttpClient ToucanHttpClient = new HttpClient();
            ToucanHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:93.0; mm:Toucan-Mod-Manager;) Gecko/20100101 Firefox/93.0");

            HttpResponseMessage ToucanHttpClientResponse = await ToucanHttpClient.GetAsync(SearchUrl);
            ToucanHttpClientResponse.EnsureSuccessStatusCode();
            string ResponseContentJsonString = await ToucanHttpClientResponse.Content.ReadAsStringAsync();
            return ResponseContentJsonString;
        }

        /// <summary>
        /// Parses the json string from GetSearchModJsonString() to a JObject and then 
        /// returns it.
        /// </summary>
        /// <param name="SearchType"></param>
        /// <param name="SearchQuery"></param>
        /// <returns></returns>
        public static async Task<JObject> GetSearchModJson(string SearchQuery) 
            => JObject.Parse(await GetSearchModJsonString(SearchQuery));

        /// <summary>
        /// JsonConvert.DeserializeObjects the json string retrieved from
        /// GetSearchModJsonString(<paramref name="SearchType"/>, <paramref name="SearchQuery"/>)
        /// </summary>
        /// <param name="SearchType"></param>
        /// <param name="SearchQuery"></param>
        /// <returns></returns>
        public static async Task<List<SpacedockAPISearchModModel>> GetSearchModModel(string SearchQuery)
            => new List<SpacedockAPISearchModModel> { JsonConvert.DeserializeObject<SpacedockAPISearchModModel>(await GetSearchModJsonString(SearchQuery)) };
    }
}