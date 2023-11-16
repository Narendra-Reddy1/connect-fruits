using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoogleSheetsData", menuName = "ScriptableObjects/GoogleSheets/GoogleSheetsData")]
public class GoogleSheetsData : ScriptableObject
{
    [Serializable]
    public struct SheetsData
    {
        public string id;
        public string sheetsURL;
    }

    public List<SheetsData> sheetDataList = new List<SheetsData>();
}

public class GoogleSheetKeys
{
    public const string LEVEL_DATA = "level_data"; 
}
