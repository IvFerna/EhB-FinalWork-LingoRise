using Godot;

public partial class AudioManager : Node
{
    public static AudioManager Instance;

    private AudioStreamPlayer _player;

    public override void _Ready()
    {
        Instance = this;

        _player = new AudioStreamPlayer
        {
            Bus = "Voice"
        };

        AddChild(_player);
    }

    public AudioStreamPlayer PlayWav(string path)
    {
        if (!FileAccess.FileExists(path))
        {
            GD.PrintErr(
                $"Audio file missing: {path}"
            );

            return null;
        }

        AudioStreamWav stream =
            AudioStreamWav.LoadFromFile(path);

        if (stream == null)
        {
            GD.PrintErr(
                $"Failed to load WAV: {path}"
            );

            return null;
        }

        AudioStreamPlayer player = new AudioStreamPlayer
        {
            Bus = "Voice"
        };

        AddChild(player);

        player.Stream = stream;

        player.Finished += () =>
        {
            if (IsInstanceValid(player))
            {
                player.QueueFree();
            }
        };

        player.Play();

        return player;
    }
}