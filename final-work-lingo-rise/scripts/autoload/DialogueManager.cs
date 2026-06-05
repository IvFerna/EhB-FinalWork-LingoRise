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

    private const string SpeechBubblePath =
        "res://scenes/ui/SpeechBubble.tscn";

    private LexiconManager _wordSystem;
    private DialoguePriority _currentPriority;
    private PackedScene _speechBubbleScene;

    private SpeechBubble _activeBubble;

    public bool IsDialogueActive =>
        GodotObject.IsInstanceValid(_activeBubble)
        && _activeBubble.IsInsideTree();

    public bool IsMenuOpen { get; private set; }
    public bool DialogueEnabled { get; private set; } = true;


    public bool CanPlayDialogue => !IsMenuOpen && DialogueEnabled;

    public override void _Ready()
    {
        Instance = this;

        _wordSystem = GetNode<LexiconManager>(
            "/root/LexiconManager"
        );

        _speechBubbleScene =
            GD.Load<PackedScene>(SpeechBubblePath);

        GD.Print(
            $"SpeechBubble scene loaded successfully: {_speechBubbleScene != null}"
        );
    }

    private bool IsManagerValid()
    {
        return GodotObject.IsInstanceValid(this)
            && IsInsideTree();
    }

    private void ClearActiveBubble()
    {
        if (GodotObject.IsInstanceValid(_activeBubble))
        {
            _activeBubble.QueueFree();
        }

        _activeBubble = null;
    }

    private bool ShouldSkipDialogue(
        DialoguePriority priority
    )
    {
        if (!IsDialogueActive)
            return false;

        return priority < _currentPriority;
    }

    private async Task PlayDialogueAudio(
        string language,
        string text
    )
    {
        await TTSService.Instance.PlayAudio(
            language,
            text
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
        {
            GD.PrintErr(
                "ShowDialogue: Cannot play dialogue right now (menu open)"
            );
            return;
        }

        if (!GodotObject.IsInstanceValid(anchor))
        {
            GD.PrintErr(
                "ShowDialogue: Dialogue anchor is invalid"
            );
            return;
        }

        if (ShouldSkipDialogue(priority))
        {
            GD.Print(
                $"Skipping dialogue. Current priority: {_currentPriority}, New priority: {priority}"
            );
            return;
        }

        ClearActiveBubble();

        _activeBubble =
            _speechBubbleScene.Instantiate<SpeechBubble>();

        anchor.AddChild(_activeBubble);

        _activeBubble.ShowText(text);

        _currentPriority = priority;

        // Small readability delay
        await ToSignal(
            GetTree().CreateTimer(0.35f),
            "timeout"
        );

        if (!IsManagerValid())
            return;

        await PlayDialogueAudio(
            language,
            text
        );

        if (!IsManagerValid())
            return;

        await ToSignal(
            GetTree().CreateTimer(2.5f),
            "timeout"
        );

        if (!IsManagerValid())
            return;

        if (!CanPlayDialogue)
        {
            StopActiveDialogue();
            return;
        }

        ClearActiveBubble();
    }

    public async Task ShowDialogueWithExposure(
        Node2D anchor,
        DialogueData dialogue,
        DialoguePriority priority = DialoguePriority.Ambient,
        Dictionary<string, string> replacements = null
    )
    {
        if (dialogue == null)
        {
            GD.PrintErr(
                "ShowDialogueWithExposure called with null DialogueData"
            );
            return;
        }

        if (_wordSystem != null &&
            dialogue.ExposedWords != null)
        {
            foreach (var word in dialogue.ExposedWords)
            {
                if (word == null)
                {
                    GD.PrintErr(
                        "ShowDialogueWithExposure: encountered null word in ExposedWords"
                    );
                    continue;
                }

                if (string.IsNullOrEmpty(word.Id))
                {
                    GD.PrintErr(
                        "ShowDialogueWithExposure: encountered exposed word with empty Id"
                    );
                    continue;
                }

                _wordSystem.RegisterExposure(
                    word.Id,
                    ExposureType.Heard
                );
            }
        }

        if (!GodotObject.IsInstanceValid(anchor))
        {
            GD.PrintErr(
                "ShowDialogueWithExposure: anchor is invalid"
            );
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

        await ShowDialogue(
            anchor,
            "es",
            finalText,
            priority
        );

        if (!IsManagerValid())
            return;
    }

    public void SetMenuOpen(bool isOpen)
    {
        IsMenuOpen = isOpen;

        if (IsMenuOpen)
        {
            StopActiveDialogue();
        }
    }

    public void ClearDialogue()
    {
        StopActiveDialogue();
    }

    private void StopActiveDialogue()
    {
        ClearActiveBubble();

        if (TTSService.Instance != null)
        {
            TTSService.Instance.Stop();
        }
    }
    public void SetDialogueEnabled(bool enabled)
    {
        DialogueEnabled = enabled;

        if (!enabled)
        {
            StopActiveDialogue();
        }
    }
}