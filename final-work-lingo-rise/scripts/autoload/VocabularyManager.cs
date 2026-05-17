using Godot;
using System.Threading.Tasks;

public partial class VocabularyManager : Node
{
    public static VocabularyManager Instance;

    public override void _Ready()
    {
        Instance = this;
    }

    public async Task PlayWord(string language, string word)
    {
        await TTSService.Instance.PlayWord(language, word);
    }
}