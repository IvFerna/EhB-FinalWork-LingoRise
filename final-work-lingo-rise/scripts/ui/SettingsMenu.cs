using Godot;
using System;

public partial class SettingsMenu : Control
{

    private void OnClosePressed()
    {
        UIManager.Instance.CloseAllMenus();
    }
}
