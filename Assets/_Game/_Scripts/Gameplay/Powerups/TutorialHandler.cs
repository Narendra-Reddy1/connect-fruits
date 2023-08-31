using AYellowpaper.SerializedCollections;
using Coffee.UIEffects;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BenStudios
{
    public class TutorialHandler : MonoBehaviour
    {

        [SerializeField] private CanvasGroup darkOverlayCanvasGroup;

        [SerializeField] private GameObject fruitBombPowerup;
        [SerializeField] private GameObject tripleBombPowerup;
        [SerializeField] private GameObject hintPowerup;
        [SerializeField] private RectTransform handRectTransfrom;
        [SerializeField] private Transform handTransfrom;
        [SerializeField] private GameObject powerupMessagePanel;
        [SerializeField] private Transform powerupMessagePanelBg;
        [SerializeField] private TextMeshProUGUI powerupMessageText;

        [Space(20)]
        [Header("Powerup Unlock Content")]
        [SerializeField] private SerializedDictionary<PowerupType, List<UIEffect>> m_powerupTypeUiEffectsDict;
        [Space(10)]
        [SerializeField] private SerializedDictionary<PowerupType, GameObject> m_powerupUnlockObjects;
        [Space(10)]
        [SerializeField] private SerializedDictionary<PowerupType, TextMeshProUGUI> m_unlockTexts;


        private Canvas m_powerupCanvas;
        private GraphicRaycaster m_powerupCanvasRaycaster;

        #region Unity Methods

        private void Awake()
        {
            _SetTutorialStates();
        }

        #endregion Unity Methods

        #region Public Methods
        public void ShowPowerupTutorial(PowerupType powerUpType)
        {
            GlobalEventHandler.RequestToPauseTimer?.Invoke(true);
            _UnlockPowerup(powerUpType);
            _RearrangeOrderAndUpdateMessage(powerUpType);
            darkOverlayCanvasGroup.gameObject.SetActive(true);
            darkOverlayCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
            {
                handRectTransfrom.gameObject.SetActive(true);
                powerupMessagePanel.SetActive(true);
            });
        }
        public void ClosePowerUpTutorial()
        {
            Destroy(m_powerupCanvasRaycaster);
            Destroy(m_powerupCanvas);
            handTransfrom.DOKill();
            m_powerupCanvas = null;
            m_powerupCanvasRaycaster = null;
            darkOverlayCanvasGroup.alpha = 0;
            darkOverlayCanvasGroup.gameObject.SetActive(false);
            handRectTransfrom.gameObject.SetActive(false);
            powerupMessagePanel.gameObject.SetActive(false);
            // AppLovinManager.appLovinManager.ShowBannerAds();
        }
        #endregion Public Methods

        #region Private Methods
        private void _UnlockPowerup(PowerupType powerupType)
        {
            m_powerupTypeUiEffectsDict[powerupType].ForEach(x => x.effectMode = EffectMode.None);
            Button powerUpButton = m_powerupTypeUiEffectsDict[powerupType][1].GetComponent<Button>();
            powerUpButton.onClick.AddListener(ClosePowerUpTutorial);
            powerUpButton.interactable = true;
            m_powerupUnlockObjects[powerupType].SetActive(false);
        }
        private void _LockPowerup(PowerupType powerupType)
        {
            m_powerupTypeUiEffectsDict[powerupType].ForEach(x => x.effectMode = EffectMode.Grayscale);
            m_powerupTypeUiEffectsDict[powerupType][1].GetComponent<Button>().interactable = false;
            m_powerupUnlockObjects[powerupType].SetActive(true);
        }
        private void _SetTutorialStates()
        {
            if (!PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_hint_powerup_tutorial_shown, false))
            {
                _LockPowerup(PowerupType.Hint);
                m_unlockTexts[PowerupType.Hint].SetText($"Unlocks at Level {Konstants.HINT_POWERUP_UNLOCK_LEVEL}");
            }
            if (!PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_triple_bomb_tutorial_shown, false))
            {
                _LockPowerup(PowerupType.TripleBomb);
                m_unlockTexts[PowerupType.TripleBomb].SetText($"Unlocks at Level {Konstants.TRIPLE_BOMB_UNLOCK_LEVEL}");
            }
            if (!PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_fruit_bomb_tutorial_shown))
            {
                _LockPowerup(PowerupType.FruitBomb);
                m_unlockTexts[PowerupType.FruitBomb].SetText($"Unlocks at Level {Konstants.FRUIT_BOMB_UNLOCK_LEVEL}");
            }
        }

        ///LOT OF REPEATED CODE HERE
        ///SHIFT TO A METHOD LATER.......
        private void _RearrangeOrderAndUpdateMessage(PowerupType powerUpType)
        {
            float handImgWidth = handRectTransfrom.sizeDelta.x / 2;
            float extraSpacingToAvoidOverlap = 150;
            darkOverlayCanvasGroup.transform.SetAsLastSibling();
            switch (powerUpType)
            {

                case PowerupType.Hint:
                    m_powerupCanvas = hintPowerup.transform.parent.gameObject.AddComponent<Canvas>();
                    m_powerupCanvasRaycaster = hintPowerup.transform.parent.gameObject.AddComponent<GraphicRaycaster>();
                    m_powerupCanvas.overrideSorting = true;
                    m_powerupCanvas.sortingOrder = 10;
                    handRectTransfrom.position = hintPowerup.transform.position;
                    handTransfrom.position = hintPowerup.transform.position;
                    handRectTransfrom.position = new Vector2(hintPowerup.transform.position.x - (1 * handImgWidth), hintPowerup.transform.position.y);
                    powerupMessagePanel.transform.position = new Vector2(hintPowerup.transform.position.x, hintPowerup.transform.position.y + extraSpacingToAvoidOverlap);
                    powerupMessageText.text = Konstants.HINT_POWERUP_MESSAGE;
                    break;

                case PowerupType.TripleBomb:
                    m_powerupCanvas = tripleBombPowerup.transform.parent.gameObject.AddComponent<Canvas>();
                    m_powerupCanvasRaycaster = tripleBombPowerup.transform.parent.gameObject.AddComponent<GraphicRaycaster>();
                    m_powerupCanvas.overrideSorting = true;
                    m_powerupCanvas.sortingOrder = 10;
                    handRectTransfrom.position = tripleBombPowerup.transform.position;
                    handTransfrom.position = tripleBombPowerup.transform.position;
                    handTransfrom.position = new Vector2(tripleBombPowerup.transform.position.x - (2 * handImgWidth), tripleBombPowerup.transform.position.y);
                    powerupMessagePanel.transform.position = new Vector2(tripleBombPowerup.transform.position.x, tripleBombPowerup.transform.position.y + extraSpacingToAvoidOverlap);
                    powerupMessageText.text = Konstants.TRIPLE_BOMB_POWERUP_MESSAGE;
                    break;
                case PowerupType.FruitBomb:
                    m_powerupCanvas = fruitBombPowerup.transform.parent.gameObject.AddComponent<Canvas>();
                    m_powerupCanvasRaycaster = fruitBombPowerup.transform.parent.gameObject.AddComponent<GraphicRaycaster>();
                    m_powerupCanvas.overrideSorting = true;
                    m_powerupCanvas.sortingOrder = 10;
                    handRectTransfrom.position = fruitBombPowerup.transform.position;
                    handTransfrom.position = fruitBombPowerup.transform.position;
                    handTransfrom.position = new Vector2(fruitBombPowerup.transform.position.x - (2 * handImgWidth), fruitBombPowerup.transform.position.y);
                    powerupMessagePanel.transform.position = new Vector2(fruitBombPowerup.transform.position.x, fruitBombPowerup.transform.position.y + extraSpacingToAvoidOverlap);
                    powerupMessageText.text = Konstants.FRUIT_BOMB_POWERUP_MESSAGE;
                    break;
            }
            handRectTransfrom.transform.SetAsLastSibling();
            powerupMessagePanel.transform.SetAsLastSibling();

            Vector3 tempPosition = new Vector3(handTransfrom.position.x, handTransfrom.position.y + handImgWidth, 0);
            handTransfrom.DOMove(tempPosition, 0.7f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }

        #endregion Private Methods
    }
}