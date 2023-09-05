using System;
using UnityEngine;

public class ProjectSetting
{

    #region PROJECT SETTING 
    public static ProjectBranch CurrentProjectBranch = ProjectBranch.DEVELOPMENT_BUILD;
    public static bool enableReporter;
    public static bool enableDebugDefine;
    public static bool enableLogWrite;
    public static int BundleVersionCode;
    private static bool isInitialized;

    #endregion

    #region DO NOT MODIFY THIS REGION
  
    public static ref bool IsProjectSettingInitialized()
    {
        return ref isInitialized;
    }

    #endregion

    #region All Setup for master Build For Final Build
    public static void EnableCoreProjectSetting(bool status)
    {
#if UPLOAD_BUILD
        Debug.unityLogger.logEnabled = false;
#else
        Debug.unityLogger.logEnabled = status;
#endif
    }



    /// <summary>
    /// This will intialize all project related Setting
    /// </summary>
    [Obsolete]
    internal static void InitializeProjectSetting(ProjectAssetManager projectAssetManager)
    {

        enableReporter = projectAssetManager.projectSettingAssets.ENABLE_REPORTER;
        enableDebugDefine = projectAssetManager.projectSettingAssets.ENABLE_DEBUG_DEFINE;
        enableLogWrite = projectAssetManager.projectSettingAssets.ENABLE_LOG_WRITE;
        CurrentProjectBranch = projectAssetManager.projectSettingAssets.CurrentProjectBranch;
        BundleVersionCode = projectAssetManager.projectSettingAssets.GetBundleVersionCode();
        MyUtils.Log("BundleVersionCode: " + BundleVersionCode);

        EnableCoreProjectSetting(enableReporter);
        int branchIndex = (int)CurrentProjectBranch;
        if (branchIndex > (int)ProjectBranch.UPLOAD_BUILD)
        {
            branchIndex = (int)ProjectBranch.DEVELOPMENT_BUILD;
        }
       
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        ///THIRD PARTY SKEY KEY SETTINGS
        

        MyUtils.Log("Device refresh rate: " + Screen.currentResolution.refreshRate);
        Application.targetFrameRate = Math.Min(60, Screen.currentResolution.refreshRate);

        isInitialized = true;
    }

    #endregion

}
