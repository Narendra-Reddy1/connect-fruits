using DG.Tweening;
using TMPro;
using UnityEngine;

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

        private Canvas m_powerupCanvas;
        public static TutorialHandler Instance { get; private set; }

        public bool isHintPowerupTutorialShown { private set; get; }
        public bool isTripleBombPowerupTutorialShown { private set; get; }
        public bool isFruitBombPowerupTutorialShown { private set; get; }

        #region Unity Methods

        private void Awake()
        {
            Instance = this;
            _SetTutorialStates();
        }
        private void OnDestroy()
        {
            Instance = null;
        }

        #endregion Unity Methods

        #region Public Methods

        public void UpdateTutorialStates()
        {
            _SetTutorialStates();
        }
        [ContextMenu("DebugTutorial")]
        public void DebugTutorai()
        {
            ClosePowerUpTutorial();
            ShowPowerupTutorial(PowerupType.FruitBomb);
        }
        [ContextMenu("DebugTripleTutorial")]
        public void DebugTRipleTutorai()
        {
            ClosePowerUpTutorial();
            ShowPowerupTutorial(PowerupType.TripleBomb);
        }
        [ContextMenu("DebugHintTutorial")]
        public void DebugHintTutorai()
        {
            ClosePowerUpTutorial();
            ShowPowerupTutorial(PowerupType.Hint);
        }
        public void ShowPowerupTutorial(PowerupType powerUpType)
        {
            _RearrangeOrderAndUpdateMessage(powerUpType);
            darkOverlayCanvasGroup.gameObject.SetActive(true);
            darkOverlayCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
            {
                handRectTransfrom.gameObject.SetActive(true);
                powerupMessagePanel.SetActive(true);
            });
        }

        [ContextMenu("StopTutorial")]
        public void ClosePowerUpTutorial()
        {
            DestroyImmediate(m_powerupCanvas);
            handTransfrom.DOKill();
            m_powerupCanvas = null;
            darkOverlayCanvasGroup.gameObject.SetActive(false);
            darkOverlayCanvasGroup.alpha = 0;
            handRectTransfrom.gameObject.SetActive(false);
            powerupMessagePanel.gameObject.SetActive(false);
            // AppLovinManager.appLovinManager.ShowBannerAds();
        }

        #endregion Public Methods

        #region Private Methods
        private void _SetTutorialStates()
        {
            isHintPowerupTutorialShown = PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_hint_powerup_tutorial_shown, false);
            isTripleBombPowerupTutorialShown = PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_triple_bomb_tutorial_shown, false);
            isFruitBombPowerupTutorialShown = PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_fruit_bomb_tutorial_shown);
        }

        private void _RearrangeOrderAndUpdateMessage(PowerupType powerUpType)
        {
            float handImgWidth = handRectTransfrom.sizeDelta.x / 2;
            float extraSpacingToAvoidOverlap = 150;
            darkOverlayCanvasGroup.transform.SetAsLastSibling();
            switch (powerUpType)
            {

                case PowerupType.Hint:
                    m_powerupCanvas = hintPowerup.transform.parent.gameObject.AddComponent<Canvas>();
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