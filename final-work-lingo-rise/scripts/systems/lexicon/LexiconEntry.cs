using Godot;

[GlobalClass]
public partial class LexiconEntry : Resource, ILexiconEntry
{
    [Export]
    public string Id { get; set; } = string.Empty;

    [Export]
    public string ForeignWord { get; set; } = string.Empty;

    [Export]
    public string NativeTranslation { get; set; } = string.Empty;

    [Export]
    public Texture2D Icon { get; set; }

    [Export]
    public AudioStream PronunciationAudio { get; set; }

    [Export]
    public WordCategory Category { get; set; }
}