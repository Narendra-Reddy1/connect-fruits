using UnityEngine;
[CreateAssetMenu(fileName = "ThirdPartySdkKeys", menuName = "ScriptableObjects/ThirdPartySdkKeys", order = 1)]
public class ThirdPartySdkKeys : ScriptableObject
{
    [SerializeField] private string _applovinSDKKey;

    [Space(10)]
    [Header("Game Analytics")]
    [SerializeField] private string _developmentGameKey;
    [SerializeField] private string _developmentSecretKey;
    [Space(5)]
    [SerializeField] private string _productionGameKey;
    [SerializeField] private string _productionSecretKey;

    public string ApplovingSDK => _applovinSDKKey;
    public string DevelopmentGameKey => _developmentGameKey;
    public string DevelopmentSecretKey => _developmentSecretKey;
    public string ProductionGameKey => _productionGameKey;
    public string ProductionSecretKey => _productionSecretKey;

    //public void SetUpBranchData(ProjectBranch projectBranch)
    //{
    //    switch (projectBranch)
    //    {
    //        case ProjectBranch.DEVELOPMENT_BUILD:
    //            SetupDevelopmentData();
    //            break;
    //        case ProjectBranch.UPLOAD_BUILD:
    //            SetupProductionData();
    //            break;
    //        default:
    //            SetupDevelopmentData();
    //            break;
    //    }
    //}

}
