using Godot;

public interface ILexiconEntry
{
    string Id { get; }
    string ForeignWord { get; }
    string NativeTranslation { get; }
    Texture2D Icon { get; }
    AudioStream PronunciationAudio { get; }
    WordCategory Category { get; }
}