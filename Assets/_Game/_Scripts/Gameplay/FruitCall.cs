using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BenStudios
{
    public class FruitCall : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Image m_timerbarImg;
        [SerializeField] private Image m_fruitImg;
        [SerializeField] private Image m_fruitOutlineImg;
        [SerializeField] private Transform m_checkMark;
        private int id;
        public int ID => id;
        #endregion Variables

        #region    Unity Methods


        #endregion Unity Methods

        #region Public Methods

        public void Init(int id, Sprite fruitSprite, Sprite outlineSprite)
        {
            this.id = id;
            m_fruitImg.sprite = fruitSprite;
            m_fruitOutlineImg.sprite = outlineSprite;
        }

        public void ActivateEntity()
        {
            m_timerbarImg.gameObject.SetActive(true);
            m_timerbarImg.fillAmount = 1;
            transform.DOScale(1, .45f).onComplete += () =>
            {
                m_timerbarImg.DOFillAmount(0, Konstants.FRUIT_CALL_TIMER).onComplete += () =>
                {
                    m_timerbarImg.gameObject.SetActive(false);
                    GlobalEventHandler.OnFruitCallIsCompleted?.Invoke();
                };
            };
        }
        public int GetQuickTimeBonusScore()
        {
            float fillPercent = m_timerbarImg.fillAmount;
            return Mathf.RoundToInt(fillPercent * Konstants.QUICK_MATCH_BONUS);
        }
        public float GetRemainingTime()
        {
            return m_timerbarImg.fillAmount;
        }
        public void PauseTimer()
        {
            m_timerbarImg.DOPause();
        }
        public void ResumeTimer()
        {
            m_timerbarImg.DOPlay();
        }

        public void DeactivateEntity()
        {
            transform.DOScale(0, .25f).onComplete += () =>
            {
                ResetAppearance();
                gameObject.SetActive(false);
            };
        }

        public void ShowMatchDoneEffect()
        {
            m_fruitImg.transform.DOScale(0, 0.2f);
            m_checkMark.DOScale(1, 0.25f).SetDelay(0.15f);
        }

        public void ResetAppearance()
        {
            m_fruitImg.transform.localScale = Vector3.one;
            m_checkMark.localScale = Vector3.zero;
        }
        //public void ChangeParent
        #endregion Public Methods

        #region Private Methods
        #endregion Private Methods

        #region Callbacks
        #endregion Callbacks


    }
}