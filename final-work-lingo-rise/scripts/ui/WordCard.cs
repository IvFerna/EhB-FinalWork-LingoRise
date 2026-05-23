using Godot;

public partial class WordCard : Control
{
	[Export] private TextureRect _icon;
	[Export] private Label _wordLabel;
	[Export] private Label _translationLabel;

	public void Setup(ILexiconEntry entry)
	{
		_icon.Texture = entry.Icon;
		_wordLabel.Text = entry.ForeignWord;
		_translationLabel.Text = entry.NativeTranslation;
		_translationLabel.Visible = false;
	}

	public async void onListenButtonPressed()
	{
		GD.Print("Listen button pressed for word: " + _wordLabel.Text);
		await VocabularyManager.Instance.PlayWord("es", _wordLabel.Text);
	}

	public void onTranslateButtonPressed()
	{
		GD.Print("Translate button pressed for word: " + _wordLabel.Text);
		_translationLabel.Visible = !_translationLabel.Visible;
	}
}
