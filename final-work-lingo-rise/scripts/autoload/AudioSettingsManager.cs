using Godot;

public partial class AudioSettingsManager : Node
{
    public static AudioSettingsManager Instance;

    private const string MASTER_BUS = "Master";
    private const string MUSIC_BUS = "Music";
    private const string VOICE_BUS = "Voice";

    public float MasterVolume { get; private set; } = 100f;
    public float MusicVolume { get; private set; } = 100f;
    public float VoiceVolume { get; private set; } = 100f;

    public override void _Ready()
    {
        Instance = this;

        ApplyAllVolumes();
    }

    public void SetMasterVolume(float value)
    {
        MasterVolume = Mathf.Clamp(value, 0f, 100f);

        ApplyVolumeToBus(MASTER_BUS, MasterVolume);
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = Mathf.Clamp(value, 0f, 100f);

        ApplyVolumeToBus(MUSIC_BUS, MusicVolume);
    }

    public void SetVoiceVolume(float value)
    {
        VoiceVolume = Mathf.Clamp(value, 0f, 100f);

        ApplyVolumeToBus(VOICE_BUS, VoiceVolume);
    }

    private void ApplyAllVolumes()
    {
        ApplyVolumeToBus(MASTER_BUS, MasterVolume);
        ApplyVolumeToBus(MUSIC_BUS, MusicVolume);
        ApplyVolumeToBus(VOICE_BUS, VoiceVolume);
    }

    private void ApplyVolumeToBus(string busName, float volume)
    {
        int busIndex = AudioServer.GetBusIndex(busName);

        if (volume <= 0)
        {
            AudioServer.SetBusMute(busIndex, true);
            return;
        }

        AudioServer.SetBusMute(busIndex, false);

        float db = Mathf.LinearToDb(volume / 100f);

        AudioServer.SetBusVolumeDb(busIndex, db);
    }
}