using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Laio
{

    /// <summary>
    /// Allows you to download a PUBLIC sheet from google as a CSV. You could export it as a different file format if you so wished. 
    /// The URL format is as such: https://docs.google.com/spreadsheets/d/{SHEET_ID}/export?format={EXPORT_FORMAT}&id={SHEET_ID}&gid={PAGE_ID}
    /// </summary>
    public static class GoogleDriveDownload
    {
        public delegate void OnCSVDownloadComplete(string value);

        static readonly HttpClient client = new HttpClient();

        //Empty class so we can start a coroutine on an empty gameobject
        private class CourotineRunner : MonoBehaviour { }

        /// <summary>
        /// Download a google sheet using UnityWebRequest and a coroutine
        /// </summary>
        /// <param name="onCompleted">Callback for when download is complete</param>
        /// <param name="sheetId"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public static Coroutine DownloadCSV(OnCSVDownloadComplete onCompleted, string sheetId, string pageId)
        {
            GameObject _downloader = new GameObject();
            CourotineRunner cr = _downloader.AddComponent<CourotineRunner>();
            string url = "https://docs.google.com/spreadsheets/d/" + sheetId
                + "/export?format=csv&id=" + sheetId + "&gid=" + pageId;

            onCompleted += ((s) => { GameObject.Destroy(_downloader); });

            return cr.StartCoroutine(DownloadData(onCompleted, url));
        }

        /// <summary>
        /// Download a google sheet using HttpClient and async
        /// </summary>
        /// <param name="sheetId"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public static async Task<string> DownloadCSVAsync(string url)
        {
            Uri uri = new Uri(url);

            string downloadData = null;

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                using HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);
                downloadData = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.LogError("Download error." + e.Message + "\n" + e.StackTrace);
            }

            return downloadData;
        }

        /// <summary>
        /// Download a google sheet using HttpClient and async
        /// </summary>
        /// <param name="sheetId"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public static async Task<string> DownloadCSVAsync(string sheetId, string pageId)
        {
            string url = "https://docs.google.com/spreadsheets/d/" + sheetId
                + "/export?format=csv&id=" + sheetId + "&gid=" + pageId;

            Uri uri = new Uri(url);

            string downloadData = null;

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                using HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);
                downloadData = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.LogError("Download error." + e.Message + "\n" + e.StackTrace);
            }

            return downloadData;
        }

        private static IEnumerator DownloadData(OnCSVDownloadComplete onCompleted, string url)
        {
            yield return new WaitForEndOfFrame();

            string downloadData = null;
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.DataProcessingError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    //Failed to download
                }
                else
                {
                    //Download successful
                    downloadData = System.Text.Encoding.Default.GetString(webRequest.downloadHandler.data);
                }
            }
            onCompleted?.Invoke(downloadData);
        }

    }
}