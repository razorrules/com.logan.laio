using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Laio
{
    public class TryGetCSV
    {

        public const int DEFAULT_TIME_OUT_DOWNLOAD_TIME = 10;

        public string FallbackFileName { get; private set; }
        public string SheetId { get; private set; }
        public string SheetPageId { get; private set; }
        public string Data { get; private set; }
        public bool IsDownloadSuccessful { get; private set; }
        public bool IsFinished { get; private set; }
        public float MaxDownloadTime { get; private set; }

        private GoogleDriveDownload.OnCSVDownloadComplete onCompleted;

        public TryGetCSV(string fallbackFileName, string sheetId, string sheetPageId)
        {
            this.FallbackFileName = fallbackFileName;
            this.SheetId = sheetId;
            this.SheetPageId = sheetPageId;
            MaxDownloadTime = DEFAULT_TIME_OUT_DOWNLOAD_TIME;
        }

        /// <summary>
        /// How long should we try downloading before aborting.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public TryGetCSV SetDownloadTimeoutTime(float maxDownloadTime) { this.MaxDownloadTime = maxDownloadTime; return this; }

        public async Task<(string data, bool successful)> GetData()
        {
            Coroutine c = Download();
            int t = 0;
            int msDelay = 100;
            while (!IsFinished)
            {
                if (t > MaxDownloadTime * 1000)
                {
                    CancelDownload();
                    //Over time limit
                    return (GetFallbackData(), IsDownloadSuccessful);
                }
                // Check if we went over the time limit. If so, load default and clear OnCompleted so we dont override
                t += msDelay;
                await Task.Delay(msDelay);
            }

            return (Data, IsDownloadSuccessful);
            //Return data.
        }

        public string GetFallbackData()
        {
            return Resources.Load<TextAsset>(FallbackFileName).text;
        }

        private Coroutine Download()
        {
            onCompleted += OnCompleted;
            Coroutine c = GoogleDriveDownload.DownloadCSV(onCompleted,
                SheetId,
                SheetPageId);
            return c;
        }

        private void CancelDownload()
        {
            IsFinished = true;
            onCompleted = null;
            IsDownloadSuccessful = false;
        }

        private void OnCompleted(string data)
        {
            if (data == null)
            {
                IsDownloadSuccessful = false;
                Data = Resources.Load<TextAsset>(FallbackFileName).text;
            }
            else
            {
                IsDownloadSuccessful = true;
                //Load fallback data
                Data = data;
            }

            onCompleted = null;
            IsFinished = true;
        }

    }
}