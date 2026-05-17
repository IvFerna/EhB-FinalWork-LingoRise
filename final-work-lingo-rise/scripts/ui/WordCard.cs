using Godot;

public partial class WordCard : Control
{
	[Export] private TextureRect _icon;
	[Export] private Label _wordLabel;

	public void Setup(ILexiconEntry entry)
	{
		_icon.Texture = entry.Icon;
		_wordLabel.Text = entry.ForeignWord;
	}

	public async void onListenButtonPressed()
	{
		GD.Print("Listen button pressed for word: " + _wordLabel.Text);
		await VocabularyManager.Instance.PlayWord("es", _wordLabel.Text);
	}
}
