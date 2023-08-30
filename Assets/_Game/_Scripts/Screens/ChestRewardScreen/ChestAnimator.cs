using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BenStudios
{
    //TODO : Refactor this class
    public class ChestAnimator : MonoBehaviour
    {


        public enum AnimationDirection
        {
            Up = 1,
            Down = -1
        }
        public class Reward
        {
            public ResourceType resourceType;
            public string rewardId;
            public Sprite rewardSprite;
            public int rewardValue;
        }



        [SerializeField] private GameObject chest;
        [SerializeField] private RectTransform chestRectTransfrom;
        [SerializeField] private Image chestImage;
        [SerializeField] private CanvasGroup chestCanvasGroup;

        [Header("Reward")]
        [SerializeField] private Transform rewardPrefab;
        [SerializeField] private Transform rewardParent;

        [Header("Chest Animation Settings")]
        [SerializeField] private AnimationCurve rewardAnimationCurve;
        [SerializeField] private AnimationCurve chestFromPosAnimationCurve;
        [SerializeField] private CanvasGroup chestShadow;

        [SerializeField] private float moveDuration = 0.2f;
        [SerializeField] private float animationHeight = 100;

        [Header("Reward Item Animating to Position")]
        [SerializeField] private Transform oneObjectPosition;
        [SerializeField] private Transform[] twoObjectPosition;
        [SerializeField] private Transform[] threeObjectPosition;
        [SerializeField] private Transform[] fiveObjectPosition;

        [Header("Chest Sprite")]
        [SerializeField] private Sprite commonChestOpenSprite;
        [SerializeField] private Sprite starChestOpenSprite;
        [SerializeField] private Sprite commonChestClosedSprite;
        [SerializeField] private Sprite starChestClosedSprite;


        [SerializeField] private ParticleSystem chestOpenBurstParticle;
        [SerializeField] private ParticleSystem chestOpenParticle;
        [SerializeField] private ParticleSystem chestBgGlowParticle;
        [SerializeField] private ParticleSystem confettiLeftParticle;
        [SerializeField] private ParticleSystem confettiRightParticle;

        [Header("To position anim setting debug")]
        public RectTransform originRectTransfrom;
        public Transform center;

        // [SerializeField]
        // private GameObject openChestGlow;
        private List<Transform> instantiatedRewards = new List<Transform>();
        private List<Reward> rewards;
        private Action onChestAnimationComplete;
        private ChestType chestType;

        //Testing 
        //private void Start()
        //{
        //    var rewardstoShow = new List<Reward>();

        //    rewardstoShow.Add(new Reward()
        //    {
        //        rewardType = RewardType.Coin,
        //        rewardSprite = SpriteManager.Instance.rewardResourcesData.GetSprite(RewardType.Coin),
        //        rewardValue = 100
        //    });
        //    rewardstoShow.Add(new Reward()
        //    {
        //        rewardType = RewardType.LifeUnlimited15Min,
        //        rewardSprite = SpriteManager.Instance.rewardResourcesData.GetSprite(RewardType.LifeUnlimited15Min),
        //        rewardValue = 15
        //    });

        //   ShowReward(rewardstoShow);
        //}
        private void OnDisable()
        {
            DOTween.KillAll();
        }
        #region Private Methods

        public void ChangeSprites(Sprite openSprite, Sprite closedSprite)
        {
            starChestClosedSprite = closedSprite;
            starChestOpenSprite = openSprite;
        }


        private void Animate()
        {
            chestCanvasGroup.alpha = 0;
            //chest.transform.localPosition = new Vector3(0, 0, 0);
            chest.transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);
            chestImage.sprite = GetClosedChestSprite();
            foreach (var item in instantiatedRewards)
            {
                item.localScale = Vector3.zero;
            }

            chestCanvasGroup.DOFade(1, 0.25f);
            chest.transform.DOScale(2.3f, 0.25f);
            chest.transform.DOLocalMoveY(150f, 0.25f);

            chest.transform.DORotate(new Vector3(0, 0, -10), 0.0625f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Unset).OnComplete(() =>
            {
                chest.transform.DORotate(new Vector3(0, 0, 10), 0.0625f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Unset);
            }).SetDelay(0.25f);

            chestShadow.DOFade(0.4f, 0.2f);

            chest.transform.DOScaleY(2.4f, 0.125f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Unset).OnComplete(() =>
            {

                chest.transform.DOScaleY(2.1f, 0.125f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Unset).OnComplete(() =>
                {
                    chestShadow.DOFade(1f, 0.25f);
                    chestShadow.transform.DOScale(1, 0.25f);
                    chest.transform.DOLocalMoveY(-381f, 0.25f);

                    chest.transform.DOScaleY(2.4f, 0.125f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Unset).OnComplete(() =>
                    {
                        chest.transform.DOScaleY(1.9f, 0.125f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Unset).OnComplete(() =>
                        {
                            //DeftAudioContainer.Instance?.PlaySFX(DeftAudioClip.AUDIO_CLIP_IN_HOME_SCREEN_GIFTOPENBOX);
                            chestImage.sprite = GetOpenChestSprite();
                            // openChestGlow.SetActive(true);
                            chestOpenBurstParticle.Play();
                            chestOpenParticle.Play();
                            chestBgGlowParticle.Play();
                            confettiLeftParticle.Play();
                            confettiRightParticle.Play();
                            ShowRewards();
                            chest.transform.DOScaleY(2.4f, 0.125f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Unset).OnComplete(() =>
                            {
                                //chest.transform.DOScaleY(2.3f, 0.125f).SetEase(Ease.Unset);
                                //Complete
                                Debug.Log("COMPLETE");
                            });

                        });
                    });
                });
            });
        }


        private void AnimateFromPosition(RectTransform originRectTransfrom, Transform origin, Transform center)
        {
            chestImage.sprite = GetClosedChestSprite();
            foreach (var item in instantiatedRewards)
            {
                item.localScale = Vector3.zero;
            }

            var tempPosition = new Vector2((origin.position.x - (Screen.width / 2) - originRectTransfrom.sizeDelta.x / 2) * 2532 / Screen.height,
                (origin.position.y - (Screen.height / 2) - originRectTransfrom.sizeDelta.y / 2) * 1170 / Screen.width);

            Debug.Log("tempPosition " + tempPosition);

            //Set the same chest size as the origin
            //chest.transform.localPosition = new Vector3(tempPosition.x, tempPosition.y - originRectTransfrom.sizeDelta.y/2, origin.localPosition.z);
            chest.transform.localPosition = tempPosition;
            chest.transform.localScale = Vector3.one;
            chestCanvasGroup.alpha = 1;

            MyUtils.DelayedCallback(0.2f, () =>
            {
                chest.transform.DOScale(2.3f, 0.5f).SetDelay(0.25f);

                StartCoroutine(MoveToTarget(chest.transform, tempPosition, center.localPosition, AnimationDirection.Down, chestFromPosAnimationCurve, () =>
                {
                    chest.transform.DORotate(new Vector3(0, 0, -10), 0.0625f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Unset).OnComplete(() =>
                    {
                        chest.transform.DORotate(new Vector3(0, 0, 10), 0.0625f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Unset);
                    }).SetDelay(0.25f);

                    chestShadow.DOFade(0.4f, 0.2f);

                    chest.transform.DOScaleY(2.4f, 0.125f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Unset).OnComplete(() =>
                    {

                        chest.transform.DOScaleY(2.1f, 0.125f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Unset).OnComplete(() =>
                        {
                            chestShadow.DOFade(1f, 0.25f);
                            chestShadow.transform.DOScale(1, 0.25f);
                            chest.transform.DOLocalMoveY(0f, 0.25f);

                            chest.transform.DOScaleY(2.4f, 0.125f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Unset).OnComplete(() =>
                            {
                                chest.transform.DOScaleY(2.0f, 0.125f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Unset).OnComplete(() =>
                                {
                                    //DeftAudioContainer.Instance?.PlaySFX(DeftAudioClip.AUDIO_CLIP_IN_HOME_SCREEN_GIFTOPENBOX);
                                    chestImage.sprite = GetOpenChestSprite();
                                    // openChestGlow.SetActive(true);
                                    chestOpenBurstParticle.Play();
                                    chestOpenParticle.Play();
                                    chestBgGlowParticle.Play();
                                    confettiLeftParticle.Play();
                                    confettiRightParticle.Play();
                                    ShowRewards();
                                    chest.transform.DOScaleY(2.4f, 0.125f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Unset).OnComplete(() =>
                                    {
                                        //chest.transform.DOScaleY(2.3f, 0.125f).SetEase(Ease.Unset);
                                        //Complete
                                    });

                                });
                            });
                        });
                    });
                }));
            });

        }

        private void ShowRewards()
        {
            switch (instantiatedRewards.Count)
            {
                case 1:
                    MoveReward(instantiatedRewards[0], chestImage.transform.localPosition, oneObjectPosition.localPosition);
                    onChestAnimationComplete.Invoke();
                    break;
                case 2:
                    MoveReward(instantiatedRewards[0], chestImage.transform.localPosition, twoObjectPosition[0].localPosition);
                    MyUtils.DelayedCallback(0.5f, () =>
                    {
                        MoveReward(instantiatedRewards[1], chestImage.transform.localPosition, twoObjectPosition[1].localPosition);
                        instantiatedRewards[1].transform.DOScale(1, 0.4f);
                        onChestAnimationComplete.Invoke();
                    });
                    break;
                case 3:
                    MoveReward(instantiatedRewards[0], chestImage.transform.localPosition, threeObjectPosition[0].localPosition);
                    MyUtils.DelayedCallback(0.5f, () =>
                    {
                        MoveReward(instantiatedRewards[1], chestImage.transform.localPosition, threeObjectPosition[1].localPosition);
                    });
                    MyUtils.DelayedCallback(1f, () =>
                    {
                        MoveReward(instantiatedRewards[2], chestImage.transform.localPosition, threeObjectPosition[2].localPosition);
                        onChestAnimationComplete.Invoke();
                    });
                    break;
                case 5:
                    MoveReward(instantiatedRewards[0], chestImage.transform.localPosition, fiveObjectPosition[0].localPosition);
                    MyUtils.DelayedCallback(0.5f, () =>
                    {
                        MoveReward(instantiatedRewards[1], chestImage.transform.localPosition, fiveObjectPosition[1].localPosition);
                    });
                    MyUtils.DelayedCallback(1f, () =>
                    {
                        MoveReward(instantiatedRewards[2], chestImage.transform.localPosition, fiveObjectPosition[2].localPosition);
                    });
                    MyUtils.DelayedCallback(1.5f, () =>
                    {
                        MoveReward(instantiatedRewards[3], chestImage.transform.localPosition, fiveObjectPosition[3].localPosition);
                    });
                    MyUtils.DelayedCallback(2f, () =>
                    {
                        MoveReward(instantiatedRewards[4], chestImage.transform.localPosition, fiveObjectPosition[4].localPosition);
                        onChestAnimationComplete.Invoke();
                    });
                    break;
            }
        }

        private void MoveReward(Transform animatingObject, Vector3 start, Vector3 end)
        {
            StartCoroutine(MoveToTarget(animatingObject, start, end, AnimationDirection.Up, rewardAnimationCurve, () =>
            {

                animatingObject?.DOLocalMoveY(animatingObject.localPosition.y + 60f, 1f).SetLoops(-1, LoopType.Yoyo);

            }));
            animatingObject?.transform.DOScale(1, 0.4f);
        }

        private IEnumerator MoveToTarget(Transform animatingObject, Vector3 start, Vector3 end, AnimationDirection direction, AnimationCurve animationCurve, Action onComplete)
        {
            //yield return new WaitForSeconds(0.3f);

            float time = 0f;
            while (time <= moveDuration)
            {
                time += Time.deltaTime;

                float linearT = time / moveDuration;
                float heightT = animationCurve.Evaluate(linearT);

                float height = Mathf.Lerp(0f, (int)direction * animationHeight, heightT);

                animatingObject.localPosition = Vector3.Lerp(start, end, linearT) + new Vector3(0f, height, 0f);

                yield return null;
            }
            onComplete.Invoke();
        }

        private void InstantiateRewards()
        {
            for (int i = 0; i < rewards.Count; i++)
            {
                var go = Instantiate(rewardPrefab, rewardParent);
                go.transform.localScale = Vector3.zero;
                var rewardItem = go.GetComponent<RewardItem>();

                rewardItem.rewardImage.sprite = rewards[i].rewardSprite;
                SetRewardValue(rewardItem, rewards[i]);
                SetRewardSize(rewardItem, rewards[i]);
                //rewardItem.tileRewardBg.SetActive(rewards[i].rewardType == RewardType.Tile);

                go.gameObject.SetActive(true);
                instantiatedRewards.Add(go);
            }
        }

        private void SetRewardValue(RewardItem rewardItem, Reward rewardData)
        {
            rewardItem.rewardAmountText.text = rewardData.rewardValue.ToString();
        }

        private void SetRewardSize(RewardItem rewardItem, Reward rewardData)
        {
            //switch (rewardData.rewardType)
            //{
            //    case RewardType.Tile:
            //        rewardItem.rootRectTransfrom.sizeDelta = new Vector2(150, 150);
            //        break;
            //    case RewardType.Theme:
            //        rewardItem.rootRectTransfrom.sizeDelta = new Vector2(300, 300);
            //        break;
            //    default:
            //        rewardItem.rootRectTransfrom.sizeDelta = new Vector2(200, 200);
            //        break;
            //}
        }

        private Sprite GetOpenChestSprite()
        {
            switch (chestType)
            {
                case ChestType.LEVEL:
                    return commonChestOpenSprite;
                case ChestType.STAR:
                    return starChestOpenSprite;
            }
            return commonChestOpenSprite;
        }

        private Sprite GetClosedChestSprite()
        {
            switch (chestType)
            {
                case ChestType.LEVEL:
                    return commonChestClosedSprite;
                case ChestType.STAR:
                    return starChestClosedSprite;
            }
            return commonChestClosedSprite;
        }

        #endregion Private Methods

        #region Public Methods
        public void ShowReward(List<Reward> rewards, ChestType chestType, Action onChestAnimationComplete)
        {
            this.onChestAnimationComplete = onChestAnimationComplete;
            this.rewards = rewards;
            this.chestType = chestType;
            InstantiateRewards();
            Animate();
        }
        public void ShowRewardFromPosition(RectTransform fromPositionRect, List<Reward> rewards, Action onChestAnimationComplete)
        {
            this.onChestAnimationComplete = onChestAnimationComplete;
            this.rewards = rewards;
            InstantiateRewards();
            AnimateFromPosition(fromPositionRect, fromPositionRect.transform, center);
        }
        public void ButtonClick()
        {
            AnimateFromPosition(originRectTransfrom, originRectTransfrom.transform, center);
        }

        #endregion Public Methods
    }
}
