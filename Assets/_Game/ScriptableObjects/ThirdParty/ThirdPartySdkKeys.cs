using UnityEngine;
[CreateAssetMenu(fileName = "ThirdPartySdkKeys", menuName = "ScriptableObjects/ThirdPartySdkKeys", order = 1)]
public class ThirdPartySdkKeys : ScriptableObject
{
    public string applovinSDKKey;

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
