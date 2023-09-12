using BenStudios.ScreenManagement;
using DG.Tweening;
using UnityEngine;


public class HowToPlayInfoPopup : PopupBase
{
    [Space(10)]
    [SerializeField] private Transform m_titleTxt;
    [SerializeField] private Transform m_blockerInforTxt;
    [Space(10)]
    [SerializeField] private Transform m_arrowStep_1;
    [SerializeField] private Transform m_arrowStep_2;
    [SerializeField] private Transform m_arrowStep_3;
    [SerializeField] private Transform m_arrowStep_4;

    [Space(10)]
    [SerializeField] private Transform m_fruitSetupOne;
    [SerializeField] private Transform m_fruitSetupTwo;
    [SerializeField] private Transform m_fruitSetupThree;
    [SerializeField] private Transform m_fruitSetupFour;

    [Space(10)]
    [SerializeField] private Transform m_tapToCloseBtn;


    public override void OnEnable()
    {
        base.OnEnable();
        _ShowTutorial();
    }
    public override void OnCloseClick()
    {
        ScreenManager.Instance.CloseLastAdditiveScreen();
    }

    private void _ShowTutorial()
    {
        m_titleTxt.DOScale(1, 1f).SetEase(Ease.OutBack).onComplete += () =>
        {
            m_arrowStep_1.DOScale(1, 1f).SetEase(Ease.OutBack).onComplete += () =>
            {
                m_fruitSetupOne.DOScale(1, 1f).SetEase(Ease.OutBack).onComplete += () =>
                {
                    m_arrowStep_2.DOScale(1, 1).SetEase(Ease.OutBack).onComplete += () =>
                    {
                        m_fruitSetupTwo.DOScale(1, 1).SetEase(Ease.OutBack).onComplete += () =>
                        {
                            m_arrowStep_3.DOScale(1, 1).SetEase(Ease.OutBack).onComplete += () =>
                            {
                                m_fruitSetupThree.DOScale(1, 1).SetEase(Ease.OutBack).onComplete += () =>
                                {
                                    m_fruitSetupFour.DOScale(1, 1).SetEase(Ease.OutBack).onComplete += () =>
                                    {
                                        m_blockerInforTxt.DOScale(1, .5f).SetEase(Ease.OutBack);
                                        m_fruitSetupFour.DOScale(1, 1).SetEase(Ease.OutBack).onComplete += () =>
                                        {
                                            m_tapToCloseBtn.DOScale(1, 1);
                                        };
                                    };
                                };
                            };
                        };
                    };
                };
            };
        };
    }

}
