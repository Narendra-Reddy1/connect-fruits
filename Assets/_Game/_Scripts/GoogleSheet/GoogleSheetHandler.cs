using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GoogleSheetHandler : MonoBehaviour
{
    [Serializable]
    public class GoogleSheetData
    {
        public string range { get; set; }
        public string majorDimension { get; set; }
        public List<List<string>> values { get; set; }
    }

    [SerializeField] GoogleSheetsData _googleSheetsData;
    [SerializeField] int _refreshIntervalInSeconds = 43200;
    [SerializeField] int _refreshCheckInterval = 300;

    private List<ISheetsModule> sheetsModuleList = new List<ISheetsModule>();
    private DateTime lastRefreshTime;
    private float currentDelta;

    #region Unity
    private void Start()
    {
        currentDelta = 0;
        lastRefreshTime = DateTime.Now;
    }

    private void Update()
    {
        currentDelta += Time.deltaTime;
        if (currentDelta >= _refreshCheckInterval)
        {
            currentDelta = 0;
            if (CanRefresh())
            {
                DoRefresh();
            }
        }
    }
    #endregion

    #region Public

    public void FetchSheetsData(string key, ISheetsModule sheetsModule, Action<GoogleSheetData> onComplete)
    {
        AddSheetsModule(sheetsModule);
        StartCoroutine(GetRequest(key, onComplete));
    }

    #endregion

    #region Private
    IEnumerator GetRequest(string key, Action<GoogleSheetData> action)
    {
        string url = _googleSheetsData.sheetDataList.Find(e => e.id.Equals(key)).sheetsURL;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = url.Split('/');
            int page = pages.Length - 1;
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    action?.Invoke(null);
                    break;
                case UnityWebRequest.Result.Success:
                    GoogleSheetData data = MyUtils.JsonToObject<GoogleSheetData>(webRequest.downloadHandler.text);
                    action?.Invoke(data);
                    break;
            }
        }
    }

    private void AddSheetsModule(ISheetsModule sheetsModule)
    {
        if (!sheetsModuleList.Contains(sheetsModule))
        {
            sheetsModuleList.Add(sheetsModule);
        }
    }

    private bool CanRefresh()
    {
        int secondsElapsed = DateTime.Now.Subtract(lastRefreshTime).Seconds;
        return secondsElapsed >= _refreshIntervalInSeconds;
    }

    private void DoRefresh()
    {
        lastRefreshTime = DateTime.Now;
        for (int i = 0; i < sheetsModuleList.Count; i++)
        {
            sheetsModuleList[i].FetchData();
        }
    }
    #endregion
}
