using Godot;

public interface ILexiconEntry
{
    string Id { get; }
    string ForeignWord { get; }
    string NativeTranslation { get; }
    Texture2D Icon { get; }
    WordCategory Category { get; }
}