using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUnit : MonoBehaviour
{
    [SerializeField] private Image m_resourceImage;
    [SerializeField] private TextMeshProUGUI m_itemCount;

    public void Init(Sprite resourceSprite, int balance)
    {
        m_resourceImage.sprite = resourceSprite;
        m_itemCount.SetText(balance.ToString());
    }
}
