using BenStudios.ScreenManagement;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace BenStudios
{
    public class GenericPowerupInfoPopup : PopupBase
    {
        [SerializeField] private Image m_powerupIcon;
        [SerializeField] private TextMeshProUGUI m_powerupInfoTxt;
        [SerializeField] private TextMeshProUGUI m_headerTxt;
        [SerializeField] private AssetReferenceTexture m_fruitBombSprite;
        [SerializeField] private AssetReferenceTexture m_tripleBombSprite;

        private const string FRUIT_BOMB_INFO = "Fruit bomb eliminates one pair irrespective of the fruit being called.";
        private const string TRIPLE_BOMB_INFO = "Triple bomb eliminates three pairs irrespective of the fruit being called.";

        private static PopupType m_popupType;
        private static Action OnPopupInitialized = default;

        public override void OnEnable()
        {
            base.OnEnable();
            OnPopupInitialized += _OnPopupInitialized;
        }
        public override void OnDisable()
        {
            OnPopupInitialized -= _OnPopupInitialized;
        }

        public static void Init(PopupType popupType)
        {
            m_popupType = popupType;
            OnPopupInitialized?.Invoke();
        }

        public void OnClickSubmit()
        {
            if (m_popupType == PopupType.FruitBomb)
                PlayerPrefsWrapper.SetPlayerPrefsBool(PlayerPrefKeys.is_fruit_bomb_tutorial_shown, true);
            else
                PlayerPrefsWrapper.SetPlayerPrefsBool(PlayerPrefKeys.is_triple_bomb_tutorial_shown, true);
            ScreenManager.Instance.CloseLastAdditiveScreen();
        }
        private void _OnPopupInitialized()
        {
            switch (m_popupType)
            {
                case PopupType.FruitBomb:
                    m_headerTxt.SetText($"FRUIT BOMB");
                    m_powerupInfoTxt.SetText(FRUIT_BOMB_INFO);
                    Addressables.LoadAssetAsync<Sprite>(m_fruitBombSprite).Completed += (handle) =>
                    {
                        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                            m_powerupIcon.sprite = handle.Result;
                    };
                    break;
                case PopupType.TripleBomb:
                    m_headerTxt.SetText($"TRIPLE BOMB");
                    m_powerupInfoTxt.SetText(TRIPLE_BOMB_INFO);
                    Addressables.LoadAssetAsync<Sprite>(m_tripleBombSprite).Completed += (handle) =>
                    {
                        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                            m_powerupIcon.sprite = handle.Result;
                    };
                    break;
            }
        }

        public enum PopupType
        {
            FruitBomb,
            TripleBomb
        }

    }
}