using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BenStudios
{
    public enum AnimationDirection
    {
        Up = 1,
        Down = -1
    }

    public enum AnimatingOrigin
    {
        Center,
        Custom
    }

    public class RewardCollectionAnimator : MonoBehaviour
    {
        [Header("This will first scale the object and then move to the target position")]
        [Header("Scale Setting")]
        [SerializeField] private float fromScale = 1;
        [SerializeField] private float toScale = 1;
        [SerializeField] private float scaleDuration = 0.5f;
        [Space]
        [Header("Reward Setting")]
        [SerializeField] private RectTransform[] animatingObjects;
        [SerializeField] private Transform parent;
        [Space]
        [Header("Move Setting")]
        [SerializeField] private AnimatingOrigin animatingOrigin = AnimatingOrigin.Custom;
        [SerializeField] private RectTransform animatingObject;
        [SerializeField] private RectTransform target;
        [SerializeField] private AnimationCurve animationCurve;
        [SerializeField] private float moveDuration = 0.6f;
        [SerializeField] private AnimationDirection direction = AnimationDirection.Down;
        [SerializeField] private float animationHeight = 300;
        [SerializeField] private Transform refStartPosition;

        private Vector3 start;
        private Vector3 end;
        private Coroutine coroutine;
        private Action onActionComplete;
        private int animatedObjectCount;

        public bool useLocalPosition = true;

        #region Unity Methods
        private void Start()
        {
            SetInitPosition();
        }

        #endregion Unity Methods

        #region Private Methods
        private IEnumerator MoveToTarget()
        {
            yield return new WaitForSeconds(0.3f);

            animatingObject.transform.DOScale(toScale, moveDuration).OnComplete(() =>
            {
                animatingObject.localScale = Vector3.zero;

                if (!useLocalPosition)
                {
                    animatingObject.gameObject.SetActive(false);
                }
            });

            float time = 0f;

            while (time <= moveDuration)
            {
                time += Time.deltaTime;

                float linearT = time / moveDuration;
                float heightT = animationCurve.Evaluate(linearT);

                float height = Mathf.Lerp(0f, (int)direction * animationHeight, heightT);
                if (useLocalPosition)
                {
                    animatingObject.localPosition = Vector3.Lerp(start, end, linearT) + new Vector3(0f, height, 0f);
                }
                else
                {
                    animatingObject.position = Vector3.Lerp(start, end, linearT) + new Vector3(0f, height, 0f);

                }

                yield return null;
            }

            animatingObject.localScale = Vector3.zero;
            onActionComplete?.Invoke();
            coroutine = null;
        }

        private IEnumerator MoveToTargetForDecor(Vector3 start, Vector3 end, float moveDuration)
        {
            yield return new WaitForSeconds(0.3f);

            animatingObject.transform.DOScale(toScale, moveDuration).OnComplete(() =>
            {
                animatingObject.localScale = Vector3.zero;
            });

            float time = 0f;

            while (time <= moveDuration)
            {
                time += Time.deltaTime;

                float linearT = time / moveDuration;
                float heightT = animationCurve.Evaluate(linearT);

                float height = Mathf.Lerp(0f, (int)direction * animationHeight, heightT);
                if (useLocalPosition)
                {
                    animatingObject.localPosition = Vector3.Lerp(start, end, linearT) + new Vector3(0f, height, 0f);
                }
                else
                {
                    animatingObject.position = Vector3.Lerp(start, end, linearT) + new Vector3(0f, height, 0f);

                }

                yield return null;
            }

            animatingObject.localScale = Vector3.zero;
            onActionComplete?.Invoke();
            coroutine = null;
        }

        private IEnumerator MoveToTargetMultipleObj(RectTransform animatingObj)
        {
            yield return new WaitForSeconds(0.3f);
            animatingObj.transform.DOScale(toScale, moveDuration).OnComplete(() =>
            {
                animatingObject.localScale = Vector3.zero;
            });

            float time = 0f;
            while (time <= moveDuration)
            {
                time += Time.deltaTime;

                float linearT = time / moveDuration;
                float heightT = animationCurve.Evaluate(linearT);

                float height = Mathf.Lerp(0f, (int)direction * animationHeight, heightT);

                animatingObj.localPosition = Vector3.Lerp(start, end, linearT) + new Vector3(0f, height, 0f);

                yield return null;
            }

            animatingObj.gameObject.SetActive(false);
            animatedObjectCount++;
            if (animatedObjectCount == animatingObjects.Length)
            {
                onActionComplete?.Invoke();
            }
            coroutine = null;
        }

        private IEnumerator InstantiateAnimatingObj(int index, RectTransform animatingObj)
        {
            yield return new WaitForSeconds(0.1f * index);
            StartCoroutine(MoveToTargetMultipleObj(animatingObj));
        }

        private void SetInitPosition()
        {
            if (useLocalPosition)
            {
                start = animatingOrigin == AnimatingOrigin.Custom ? animatingObject.localPosition : animatingObject.localPosition;
                end = target.localPosition;
            }
            else
            {
                start = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                end = target.position;

            }
        }
        #endregion Private Methods

        #region Public Methods
        public void ShowRewardCollectionAnimation(Action onActionComplete)
        {

            for (int i = 0; i < animatingObjects.Length; i++)
            {
                animatingObjects[i].localPosition = refStartPosition.localPosition;
            }



            this.onActionComplete = onActionComplete;
            if (coroutine == null)
            {
                animatingObject.localPosition = start;
                animatingObject.transform.DOScale(fromScale, scaleDuration).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    if (animatingObjects.Length > 1)
                    {

                        animatedObjectCount = 0;
                        MoveToTargetMultipleObj(animatingObject);
                        for (int i = 1; i <= animatingObjects.Length; i++)
                        {
                            animatingObjects[i - 1].localScale = new Vector3(fromScale, fromScale, fromScale);
                            animatingObjects[i - 1].gameObject.SetActive(true);
                            StartCoroutine(InstantiateAnimatingObj(i, animatingObjects[i - 1]));
                        }
                    }
                    else
                    {
                        if (!useLocalPosition)
                        {
                            animatingObject.transform.position = start;
                            animatingObject.gameObject.SetActive(true);
                        }
                        coroutine = StartCoroutine(MoveToTarget());
                    }
                });
            }
        }

        public void ShowAmountDEductionAnimationForRoomDecor(Vector3 endPos, float duration, Action onActionComplete)
        {
            this.onActionComplete = onActionComplete;
            if (coroutine == null)
            {
                animatingObject.localPosition = target.position;
                animatingObject.transform.DOScale(fromScale, scaleDuration).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    if (!useLocalPosition)
                    {
                        animatingObject.transform.position = target.position;
                        animatingObject.gameObject.SetActive(true);
                    }
                    coroutine = StartCoroutine(MoveToTargetForDecor(target.position, endPos, duration));
                });
            }
        }
        #endregion Public Methods
    }
}
