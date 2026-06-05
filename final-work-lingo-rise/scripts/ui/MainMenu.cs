using Godot;
using System;

public partial class MainMenu : Control
{

	public override void _Ready()
	{
		GD.Print(UIManager.Instance);
	}

	private void OnPlayPressed()
	{
		UIManager.Instance.OnPlayPressed();
	}

	private void OnSettingsPressed()
	{
		UIManager.Instance.OpenSettings();
	}

	private void OnLexiconPressed()
	{
		UIManager.Instance.OpenLexicon();
	}

	private void OnClosePressed()
	{
		GD.Print("Close button pressed.");
		GD.Print(UIManager.Instance);
		UIManager.Instance.CloseAllMenus();
	}

}
