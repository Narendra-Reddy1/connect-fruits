using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FruitFrenzy;

public class DebugNode : MonoBehaviour
{
    public TextMeshProUGUI fCostTxt;
    public TextMeshProUGUI gCostTxt;
    public TextMeshProUGUI hCostTxt;

    public FruitEntity fruitEntity;
    private void Start()
    {
        hCostTxt.text = string.Empty;
        gCostTxt.text = string.Empty;
        fCostTxt.text = $"{fruitEntity.Row}x{fruitEntity.Column}";
    }
   
}
