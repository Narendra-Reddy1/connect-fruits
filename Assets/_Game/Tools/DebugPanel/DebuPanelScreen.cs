using BenStudios.ScreenManagement;
public class DebuPanelScreen : ScreenBase
{
    public override void OnCloseClick()
    {
        ScreenManager.Instance.CloseLastAdditiveScreen();
    }
}
