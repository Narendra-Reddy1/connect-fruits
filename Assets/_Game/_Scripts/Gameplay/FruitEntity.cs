using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using AYellowpaper.SerializedCollections;
using System;
using Unity.VisualScripting;

/*
 Selecting fruitSprite
unselecting fruitSprite.
sending the selected fruitSprite.
 
GCost: Distance from start entity to currentEntity.
HCost:Distance from currentEntity to EndEntity;
FCost: Sum of GCost & HCost. (Total Distance b/w Start to End Entites
 */

namespace BenStudios
{
    public class FruitEntity : MonoBehaviour, IPointerClickHandler
    {

        #region Variables
        [SerializeField] private Image m_fruitImage;
        [SerializeField] private Image m_fruitOutline;
        [SerializeField] private Image m_gridCellBG;
        [SerializeField] private GameObject m_selectionEffect;
        [SerializeField] private Image m_starImage;
        [SerializeField] private Transform m_fruitBombImage;
        [SerializeField] private CanvasGroup m_entityCanvasGroup;
        [SerializeField] private Sprite m_yellowStarImage;
        [SerializeField] private Sprite m_redStarImage;
        public SerializedDictionary<Direction, FruitEntity> neighbours;
        private int m_id;
        private int row, column;
        private bool m_isSelected;
        private bool m_isDestroyed;
        private bool m_isInVisibleEntity;
        private bool m_isCleared;
        public int ID => m_id;
        public int Row => row;
        public int Column => column;
        public bool IsDestroyed => m_isDestroyed;
        #endregion Variables

        #region UnityMethods
        #endregion UnityMethods

        #region Public Methods
        public void Init(Sprite fruitSprite, Sprite fruitOutlineSprite, int id, int row, int column, bool isInvisibleCell = false)
        {
            m_id = id;
            this.row = row;
            this.column = column;
            if (id == -1 || isInvisibleCell)
            {
                m_isInVisibleEntity = isInvisibleCell;
                m_entityCanvasGroup.alpha = 0;
                m_isDestroyed = true;
                return;
            }
            m_fruitImage.sprite = fruitSprite;
            m_fruitOutline.sprite = fruitOutlineSprite;
        }
        public void SetupNeighbours(FruitEntity[,] fruitArray)
        {
            if (row - 1 >= 0)//UP
                if (fruitArray[row - 1, column] != null)
                {
                    neighbours.Add(Direction.Up, fruitArray[row - 1, column]);
                }
            if (column - 1 >= 0)//LEFT
                if (fruitArray[row, column - 1] != null)
                {
                    neighbours.Add(Direction.Left, fruitArray[row, column - 1]);
                }
            if (row + 1 < Konstants.ROW_SIZE)//DOWN
                if (fruitArray[row + 1, column] != null)
                {
                    neighbours.Add(Direction.Down, fruitArray[row + 1, column]);
                }
            if (column + 1 < Konstants.COLUMN_SIZE)//RIGHT
                if (fruitArray[row, column + 1] != null)
                {
                    neighbours.Add(Direction.Right, fruitArray[row, column + 1]);
                }
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_isDestroyed || m_isInVisibleEntity) return;
            if (m_isSelected)
                UnSelectEntity();
            else
                SelectEntity();
            GlobalEventHandler.RequestToPlaySFX?.Invoke(AudioID.FruitTileClickSFX);
        }
        public void SelectEntity()
        {
            m_isSelected = true;
            CanShowSelectedEffect(true);
            //Trigger Fruit Selected Event here.
            GlobalEventHandler.OnFruitEntitySelected?.Invoke(this);
        }
        public void UnSelectEntity()
        {
            m_isSelected = false;
            CanShowSelectedEffect(false);
            //Trigger Fruit Unselect Event here.
            GlobalEventHandler.OnFruitEntityUnSelected?.Invoke(this);
        }
        public void ResetEntity()
        {
            if (m_isInVisibleEntity) return;
            m_isSelected = false;
            CanShowSelectedEffect(false);
            m_isDestroyed = false;
        }
        public void CanShowSelectedEffect(bool show)
        {
            if (m_isInVisibleEntity) return;
            m_selectionEffect.SetActive(show);
        }
        public void ShowMatchEffect(float delay = 0f, System.Action onComplete = null)
        {
            m_isDestroyed = true;
            m_fruitImage.transform.DOScale(1.25f, 0.35f).SetDelay(delay).onComplete += () =>
            {
                ShrinkAndDestroy(onComplete/*ShowBalstEffect() and EnableOutLine()*/);
            };
        }
        public void ShrinkAndDestroy(Action onComplete = null)
        {
            m_isDestroyed = true;
            m_fruitImage.transform.DOScale(0, .65f).onComplete += () =>
            {
                m_fruitImage.gameObject.SetActive(false);
                CanShowSelectedEffect(false);
                onComplete?.Invoke();
            };
            ShowFruitOutline();
        }
        public void ShowFruitOutline(Action onComplete = null)
        {
            m_fruitOutline.transform.DOScale(Vector3.one, .65f).onComplete += () => { onComplete?.Invoke(); };
        }
        public void ShowRowOrColumnGotClearedEffect(float delay = 0)
        {
            if (m_isInVisibleEntity) return;
            if (m_isCleared)
                SetAsColumAndRowCleared(m_redStarImage);
            else
            {
                m_isCleared = true;
                m_starImage.transform.DOScale(Vector3.one * 0.3f, .2f).SetEase(Ease.Linear);
                SetAsColumAndRowCleared(m_yellowStarImage);
            }
            void SetAsColumAndRowCleared(Sprite starSprite)
            {
                m_starImage.sprite = starSprite;

                Canvas starCanvas = m_starImage.GetComponent<Canvas>();
                if (starCanvas == null)
                    starCanvas = m_starImage.AddComponent<Canvas>();

                Canvas fruitCanvas = m_fruitOutline.GetComponent<Canvas>();
                if (fruitCanvas == null)
                    fruitCanvas = m_fruitOutline.AddComponent<Canvas>();

                if (starCanvas)
                {
                    starCanvas.overrideSorting = true;
                    starCanvas.sortingOrder = 101;
                }
                if (fruitCanvas)
                {
                    fruitCanvas.overrideSorting = true;
                    fruitCanvas.sortingOrder = 100;
                }
                if (!DOTween.IsTweening(m_fruitOutline.transform.localScale, true))
                {

                    m_fruitOutline.transform.DOPunchScale(Vector3.one * 1.5f, 1f, 1).SetEase(Ease.Linear).SetDelay(delay).onComplete += () =>
                    {
                        m_fruitOutline.transform.localScale = Vector3.one;
                        Destroy(fruitCanvas);
                        Destroy(starCanvas);
                    };
                }
                if (!DOTween.IsTweening(m_starImage.transform.rotation, true))
                {
                    m_starImage.transform.DORotate(Vector3.forward * 360f, 0.65f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetDelay(delay / 2);
                }
            }
        }
        public void ShowEntireBoardClearedEffect(float delay = 0)
        {
            if (m_isInVisibleEntity) return;
            m_starImage.sprite = m_redStarImage;
            m_starImage.rectTransform.DOAnchorPos(Vector3.zero, 1f).SetDelay(delay);
            m_starImage.transform.DOScale(Vector3.one, 1f).SetDelay(delay);
        }

        public void ShowFruitBombEffect()
        {
            m_fruitBombImage.DOScale(Vector3.one, .5f).onComplete += () =>
            {
                m_fruitBombImage.DOScale(Vector3.zero, 0.45f);
                _ShowBlastEffect();
            };
        }
        public void HighlightFruitntity(bool canHighlight)
        {
            if (canHighlight)
            {
                m_fruitImage.transform.DOPunchRotation(Vector3.forward * 20, 1, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad).SetId(1);
                m_fruitImage.transform.DOScale(1.125f, 1);
            }
            else
            {
                //DOTween.Kill(1);
                m_fruitImage.transform.DOKill();
                m_fruitImage.transform.localScale = Vector3.one;
                m_fruitImage.transform.rotation = Quaternion.identity;
            }
        }
        #endregion Public Methods

        #region Private Methods

        private void _ShowBlastEffect()
        {
            //Show blast Particles here
            ShowFruitOutline();
        }

        #endregion Private Methods

        #region Callbacks

        #endregion Callbacks

    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }
}