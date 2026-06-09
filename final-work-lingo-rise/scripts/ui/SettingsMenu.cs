using Godot;
using System;

public partial class SettingsMenu : Control
{
    [Export] private HSlider _masterSlider;
    [Export] private HSlider _musicSlider;
    [Export] private HSlider _voiceSlider;

    [Export] private TextureRect _masterIcon;
    [Export] private TextureRect _musicIcon;
    [Export] private TextureRect _voiceIcon;
    [Export] private Texture2D _muteIcon;
    [Export] private Texture2D _lowIcon;
    [Export] private Texture2D _mediumIcon;
    [Export] private Texture2D _highIcon;

    [Export] private Label _volumeLabel;
    [Export] private Label _nativeLabel;
    [Export] private Label _foreignLabel;

    [Export] private OptionButton _nativeLanguageOption;
    // [Export] private OptionButton _foreignLanguageOption;

    public override void _Ready()
    {
        LanguageManager.Instance.LanguageChanged += RefreshTexts;

        RefreshTexts();

        _masterSlider.ValueChanged += OnMasterVolumeChanged;
        _musicSlider.ValueChanged += OnMusicVolumeChanged;
        _voiceSlider.ValueChanged += OnVoiceVolumeChanged;

        _masterSlider.Value = AudioSettingsManager.Instance.MasterVolume;
        _musicSlider.Value = AudioSettingsManager.Instance.MusicVolume;
        _voiceSlider.Value = AudioSettingsManager.Instance.VoiceVolume;

        _volumeLabel.Text = LanguageManager.Instance.GetText("settings_volume_label");
        _nativeLabel.Text = LanguageManager.Instance.GetText("settings_native_language_label");
        _foreignLabel.Text = LanguageManager.Instance.GetText("settings_foreign_language_label");

    }
    private void OnMasterVolumeChanged(double value)
    {
        AudioSettingsManager.Instance.SetMasterVolume((float)value);

        UpdateVolumeIcon(_masterIcon, (float)value);
    }

    private void OnMusicVolumeChanged(double value)
    {
        AudioSettingsManager.Instance.SetMusicVolume((float)value);

        // UpdateVolumeIcon(_musicIcon, (float)value);
    }

    private void OnVoiceVolumeChanged(double value)
    {
        AudioSettingsManager.Instance.SetVoiceVolume((float)value);

        // UpdateVolumeIcon(_voiceIcon, (float)value);
    }
    private void UpdateVolumeIcon(TextureRect icon, float volume)
    {
        if (volume <= 0)
        {
            icon.Texture = _muteIcon;
        }
        else if (volume <= 25)
        {
            icon.Texture = _lowIcon;
        }
        else if (volume <= 50)
        {
            icon.Texture = _mediumIcon;
        }
        else
        {
            icon.Texture = _highIcon;
        }
    }
    private void OnClosePressed()
    {
        UIManager.Instance.CloseAllMenus();
    }

    private void onBackPressed()
    {
        UIManager.Instance.CloseCurrentSubmenu(this);
    }

    private void OnResetProgressPressed()
    {
        Main.Instance.ResetGame();
        GD.Print("Game progress reset.");
        UIManager.Instance.CloseCurrentSubmenu(this);
    }

    private void OnNativeLanguageChanged(int index)
    {
        string selectedLanguage = _nativeLanguageOption.GetItemText(index);
        LanguageManager.Instance.SetLanguage(selectedLanguage);
        GD.Print($"Native language changed to: {selectedLanguage}");
    }

    public override void _ExitTree()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.LanguageChanged -= RefreshTexts;
    }

    private void RefreshTexts()
    {
        _volumeLabel.Text =
            LanguageManager.Instance.GetText("settings_volume_label");

        _nativeLabel.Text =
            LanguageManager.Instance.GetText("settings_native_language_label");

        _foreignLabel.Text =
            LanguageManager.Instance.GetText("settings_foreign_language_label");
    }

}
