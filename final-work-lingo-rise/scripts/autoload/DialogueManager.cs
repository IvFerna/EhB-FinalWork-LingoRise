using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

public enum DialoguePriority
{
    Ambient = 0,
    Gameplay = 1
}

public partial class DialogueManager : Node
{
    public static DialogueManager Instance;
    private LexiconManager _wordSystem;
    private DialoguePriority _currentPriority;
    private PackedScene _speechBubbleScene;

    private SpeechBubble _activeBubble;

    public bool IsDialogueActive
    => _activeBubble != null
       && _activeBubble.IsInsideTree();

    public bool IsMenuOpen { get; private set; }
    public bool CanPlayDialogue => !IsMenuOpen;

    public override void _Ready()
    {
        Instance = this;

        _wordSystem = GetNode<LexiconManager>(
            "/root/LexiconManager"
        );

        _speechBubbleScene =
            GD.Load<PackedScene>(
                "res://scenes/ui/SpeechBubble.tscn"
            );
    }

    public async Task ShowDialogue(
        Node2D anchor,
        string language,
        string text,
        DialoguePriority priority
    )
    {
        if (!CanPlayDialogue)
            return;

        if (IsDialogueActive)
        {
            if (priority < _currentPriority)
            {
                return;
            }

            _activeBubble.QueueFree();
            _activeBubble = null;
        }

        _activeBubble = _speechBubbleScene.Instantiate<SpeechBubble>();
        anchor.AddChild(_activeBubble);

        _activeBubble.ShowText(text);
        _currentPriority = priority;
        // Small readability delay
        await ToSignal(
            GetTree().CreateTimer(0.35f),
            "timeout"
        );

        if (!CanPlayDialogue)
        {
            StopActiveDialogue();
            return;
        }

        await TTSService.Instance.PlayAudio(
            language,
            text
        );

        if (!CanPlayDialogue)
        {
            StopActiveDialogue();
            return;
        }
        await ToSignal(
            GetTree().CreateTimer(2.5f),
            "timeout"
        );


        if (_activeBubble != null)
        {
            _activeBubble.QueueFree();
            _activeBubble = null;
        }
    }

    public async Task ShowDialogueWithExposure(
        Node2D anchor,
        DialogueData dialogue,
        DialoguePriority priority = DialoguePriority.Ambient,
        Dictionary<string, string> replacements = null)
    {
        if (dialogue == null)
        {
            GD.PrintErr("ShowDialogueWithExposure called with null DialogueData");
            return;
        }

        if (_wordSystem != null && dialogue.ExposedWords != null)
        {
            foreach (var word in dialogue.ExposedWords)
            {
                _wordSystem.RegisterExposure(
                    word.Id,
                    ExposureType.Heard
                );
            }
        }

        if (anchor == null)
        {
            GD.PrintErr("ShowDialogueWithExposure: anchor is null, aborting dialogue display");
            return;
        }

        string finalText = dialogue.Text;

        if (replacements != null)
        {
            foreach (var pair in replacements)
            {
                finalText = finalText.Replace(
                    $"{{{pair.Key}}}",
                    pair.Value
                );
            }
        }

        await DialogueManager.Instance.ShowDialogue(
            anchor,
            "es",
            finalText,
            priority
        );
    }

    public void SetMenuOpen(bool isOpen)
    {
        IsMenuOpen = isOpen;

        if (IsMenuOpen)
        {
            StopActiveDialogue();
        }
    }

    private void StopActiveDialogue()
    {
        if (_activeBubble != null)
        {
            _activeBubble.QueueFree();
            _activeBubble = null;
        }

        // Optional:
        // stop TTS audio immediately if your service supports it
        TTSService.Instance.Stop();
    }
}