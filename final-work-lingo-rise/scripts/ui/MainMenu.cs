using Godot;
using System;

public partial class MainMenu : Control
{
	[Export] private Button _playButton;
	[Export] private Button _settingsButton;
	[Export] private Button _lexiconButton;

	public override void _Ready()
	{
		LanguageManager.Instance.LanguageChanged += RefreshTexts;

		GD.Print(UIManager.Instance);
		_playButton.Text = LanguageManager.Instance.GetText("play");
		_settingsButton.Text = LanguageManager.Instance.GetText("settings");
		_lexiconButton.Text = LanguageManager.Instance.GetText("lexicon");
	}

	private void RefreshTexts()
	{
		_playButton.Text = LanguageManager.Instance.GetText("play");
		_settingsButton.Text = LanguageManager.Instance.GetText("settings");
		_lexiconButton.Text = LanguageManager.Instance.GetText("lexicon");
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
