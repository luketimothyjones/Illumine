using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace UpdateManager
{
    public class UpdateManager
    {
        private const string releasesLink = "https://api.github.com/repos/luketimothyjones/illumine/releases";
        private static readonly HttpClient client = new HttpClient();
        private string updateDownloadURL = null;

        public async Task<bool> CheckForUpdate()
        {
            // Get version of current executable
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string installedVersion = fileVersionInfo.ProductVersion;

            Console.WriteLine("Installed verson: " + installedVersion);

            // Pull releases list from GitHub
            HttpResponseMessage githubResponse;
            using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, releasesLink))
            {
                requestMessage.Headers.Add("User-Agent", "Illumine");
                githubResponse = await client.SendAsync(requestMessage);
            }

            string githubMessage = await githubResponse.Content.ReadAsStringAsync();

            if (!githubResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("Error in response from GitHub: HTTP " + githubResponse.StatusCode);
                return false;
            }

            List<Release> githubReleases;
            Release mostRecentRelease;
            try
            {
                githubReleases = JsonConvert.DeserializeObject<List<Release>>(githubMessage);
                mostRecentRelease = githubReleases[0];
            }
            catch (JsonSerializationException)
            {
                Console.WriteLine("JSON deserialization failed from GitHub releases");
                return false;
            }

            string mostRecentReleaseVersion = mostRecentRelease.tag_name.Replace("-beta", "").Replace("v", "");
            if (installedVersion == mostRecentReleaseVersion)
            {
                Console.WriteLine("Illumine is up-to-date");
                return false;
            }

            updateDownloadURL = mostRecentRelease.assets[0].browser_download_url;

            return true;
        }

        public async Task<bool> DoUpdate()
        {
            Task<bool> updateTask = Task.Run(CheckForUpdate);
            await updateTask;

            bool needsUpdate = updateTask.Result;
            if (!needsUpdate)
            {
                // Either we don't need an update or something went wrong
                Console.WriteLine("Illumine is already up-to-date");
                return false;
            }

            // Ask the user if they want the update
            DialogResult shouldDoUpdate = MessageBox.Show("There is an update available for Illumine, would you like to install it?", "Illumine Updater", MessageBoxButtons.YesNo);
            if (shouldDoUpdate == DialogResult.No)
            {
                return false;
            }

            // We need to update
            HttpResponseMessage githubResponse;
            using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, updateDownloadURL))
            {
                Console.WriteLine("Update required\nDownloading Illumine installer...");
                requestMessage.Headers.Add("User-Agent", "Illumine");
                githubResponse = await client.SendAsync(requestMessage);
            }

            // Download the installer
            string installerFilePath = string.Format(@"{0}Illumine Setup.exe", Path.GetTempPath());
            File.Delete(installerFilePath);  // Make sure we don't throw an IO error due to an existing file

            try
            {
                using (FileStream fs = new FileStream(installerFilePath, FileMode.CreateNew))
                {
                    await githubResponse.Content.CopyToAsync(fs);
                    Console.WriteLine("Download complete");
                }
            }
            catch (IOException)
            {
                Console.WriteLine("IOException while trying to download update");
                return false;
            }

            // Run update and exit
            Process.Start(installerFilePath);
            Environment.Exit(0);

            return true;
        }
    }

    #region JSON schemas

    #region Releases schema
    public class Author
    {
        public string login { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
    }

    public class Uploader
    {
        public string login { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
    }

    public class Asset
    {
        public string url { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string name { get; set; }
        public object label { get; set; }
        public Uploader uploader { get; set; }
        public string content_type { get; set; }
        public string state { get; set; }
        public int size { get; set; }
        public int download_count { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string browser_download_url { get; set; }
    }

    public class Release
    {
        public string url { get; set; }
        public string assets_url { get; set; }
        public string upload_url { get; set; }
        public string html_url { get; set; }
        public int id { get; set; }
        public Author author { get; set; }
        public string node_id { get; set; }
        public string tag_name { get; set; }
        public string target_commitish { get; set; }
        public string name { get; set; }
        public bool draft { get; set; }
        public bool prerelease { get; set; }
        public DateTime created_at { get; set; }
        public DateTime published_at { get; set; }
        public List<Asset> assets { get; set; }
        public string tarball_url { get; set; }
        public string zipball_url { get; set; }
        public string body { get; set; }
    }

    #endregion
    #endregion
}
