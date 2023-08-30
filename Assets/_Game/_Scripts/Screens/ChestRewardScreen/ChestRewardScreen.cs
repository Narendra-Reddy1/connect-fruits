using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BenStudios.ScreenManagement;

namespace BenStudios
{
    public enum ChestType
    {
        LEVEL,
        STAR,
    }
    public class ChestRewardScreen : ScreenBase
    {
        public static ChestRewardScreen Instance { private set; get; }
        [SerializeField] ChestAnimator chestAnimator;
        [SerializeField] Button claimButton;
        [SerializeField] ParticleSystem claimButtonParticleOnClick;

        [SerializeField] CanvasGroup bg;
        [SerializeField] CanvasGroup transaprentBg;
        [SerializeField] CanvasGroup screenCanvasGroup;

        private List<ChestAnimator.Reward> rewards;
        private Action onClaimClickAction;
        private Action onNotClaimClickAction;
        private ChestType chestType;

        [SerializeField] private Vector3 glowOffsetForTimedEvent;
        public ChestAnimator p_ChestAnimator => chestAnimator;


        #region Unity Methods
        private void Awake()
        {
            Instance = this;
        }
        private void OnDestroy()
        {
            Instance = null;
        }
        #endregion Unity Methods

        #region Private Methods

        private void ShowClaimButton()
        {
            MyUtils.DelayedCallback(0.3f, () =>
            {
                claimButton.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    claimButtonParticleOnClick.Play();
                    claimButton.transform.DOScale(1.1f, 1).SetLoops(-1, LoopType.Yoyo);
                });

            });
        }

        private void OnChestAnimationComplete()
        {
            ShowClaimButton();
        }
        #endregion Private Methods

        #region Public Methods
        public void ShowChestReward(List<ChestAnimator.Reward> rewards, ChestType chestType, Action onClaimClickAction, Action onNotClaimClickAction/*, ChestType chestType*/)
        {
            this.chestType = chestType;
            transaprentBg.alpha = 1;
            this.rewards = rewards;
            this.onClaimClickAction = onClaimClickAction;
            this.onNotClaimClickAction = onNotClaimClickAction;
            chestAnimator.ShowReward(rewards, chestType, OnChestAnimationComplete);
        }

        //public void ShowChestRewardFromPosition(RectTransform fromPositionRect, List<ChestAnimator.Reward> rewards, Action onClaimClickAction/*, ChestType chestType*/)
        //{
        //    //this.chestType = chestType;
        //    bg.gameObject.SetActive(true);

        //    bg.DOFade(1, 0.4f);
        //    transaprentBg.DOFade(1, 0.4f).OnComplete(() =>
        //    {
        //        // rewardHeading.gameObject.SetActive(true);
        //        //header.SetActive(true);
        //    });

        //    this.rewards = rewards;
        //    this.onClaimClickAction = onClaimClickAction;
        //    chestAnimator.ShowRewardFromPosition(fromPositionRect, rewards, OnChestAnimationComplete/*, chestType*/);
        //    GiveReward();
        //}

        public void OnClickClaim()
        {
            claimButton.interactable = false;
            claimButton.DOKill();
            onClaimClickAction?.Invoke();
            try
            {
                screenCanvasGroup.DOFade(0, 1f).OnComplete(() =>
                {
                    ScreenManager.Instance.CloseLastAdditiveScreen();
                });
            }
            catch (System.Exception e)
            {
                MyUtils.Log(e);
            }
        }

        public override void OnCloseClick()
        {
            onNotClaimClickAction.Invoke();
        }

        #endregion Public Methods
    }
}
