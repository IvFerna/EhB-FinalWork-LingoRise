using Godot;

public partial class UIManager : CanvasLayer
{
    public static UIManager Instance;

    [Export] private Control _mainMenu;
    [Export] private Control _settingsMenu;
    [Export] private Control _lexiconMenu;

    public override void _Ready()
    {
        Instance = this;

        CloseAllMenus();
    }

    public void ToggleMainMenu()
    {
        GD.Print("Toggling Main Menu: " + !_mainMenu.Visible);
        _mainMenu.Visible = !_mainMenu.Visible;
        DialogueManager.Instance.SetMenuOpen(_mainMenu.Visible);
    }

    public void OpenLexicon()
    {
        CloseAllMenus();
        _lexiconMenu.Visible = true;
        DialogueManager.Instance.SetMenuOpen(true);
    }

    public void OpenSettings()
    {
        CloseAllMenus();
        _settingsMenu.Visible = true;
        DialogueManager.Instance.SetMenuOpen(true);
    }

    public void CloseAllMenus()
    {
        GD.Print("Closing all menus.");
        _mainMenu.Visible = false;
        _settingsMenu.Visible = false;
        _lexiconMenu.Visible = false;
        DialogueManager.Instance.SetMenuOpen(false);
    }

    public void CloseCurrentSubmenu(Control menu)
    {
        menu.Visible = false;
        _mainMenu.Visible = true;
        DialogueManager.Instance.SetMenuOpen(true);
    }
}