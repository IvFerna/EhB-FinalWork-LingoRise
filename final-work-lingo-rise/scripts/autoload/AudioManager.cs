using Godot;

public partial class AudioManager : Node
{
    public static AudioManager Instance;

    private AudioStreamPlayer _player;

    public override void _Ready()
    {
        Instance = this;

        _player = new AudioStreamPlayer();

        AddChild(_player);
    }

    public void PlayWav(string path)
    {
        AudioStreamWav stream =
            AudioStreamWav.LoadFromFile(path);

        _player.Stream = stream;

        _player.Play();
    }
}