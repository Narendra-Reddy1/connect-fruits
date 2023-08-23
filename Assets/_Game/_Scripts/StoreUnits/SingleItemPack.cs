using BenStudios;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleItemPack : MonoBehaviour
{
    [SerializeField] private Image m_itemIcon;
    [SerializeField] private TextMeshProUGUI m_itemCount;
    [SerializeField] private TextMeshProUGUI m_itemPriceTxt;
    [SerializeField] private TextureDatabase m_textureDatabase;
    public void Init(SinglePackData singlePackData)
    {
        m_itemIcon.sprite = m_textureDatabase.GetSpriteWithID(singlePackData.resourceType);
        m_itemCount.SetText(singlePackData.itemCount.ToString());
    }

    public void Buy()
    {

    }

    private void _AssignProductID()
    {

    }
    private void _SetLocalizedPrice()
    {

    }
}
