using Godot;
using System;
using System.Collections.Generic;

public partial class LanguageManager : Node
{
    public static LanguageManager Instance { get; private set; }

    public string CurrentLanguage { get; private set; } = "nl"; // Default language

    public event Action LanguageChanged;

    public override void _Ready()
    {
        if (Instance != null)
        {
            GD.PrintErr("Multiple instances of LanguageManager detected. This should not happen.");
            QueueFree();
            return;
        }
        Instance = this;
    }

    public void SetLanguage(string languageCode)
    {
        CurrentLanguage = languageCode;
        LanguageChanged?.Invoke();
        GD.Print($"Language set to: {CurrentLanguage}");
    }

    private readonly Dictionary<string, Dictionary<string, string>> _translations =
    new()
    {
            {
                "nl", new()
                {
                    { "play", "Spelen" },
                    { "settings", "Instellingen" },
                    { "quit", "Afsluiten" },
                    { "quest_explore_town", "Verken de stad" },
                    { "quest_find_item", "Vind {0}" },
                    { "quest_return_to_npc", "Keer terug naar de npc" },
                    { "quest_buy_breakfast", "Koop ontbijt" },
                    { "quest_talk_baker", "Praat met de bakker" },

                    { "quest_serve_customers", "Bedien de klanten" },
                    { "quest_talk_customer", "Praat met een klant" },

                    { "quest_completed", "Voltooid!" },

                    { "settings_volume_label", "Volume" },
                    { "settings_native_language_label", "Moedertaal: " },
                    { "settings_foreign_language_label", "Vreemde taal: " }
                }
            },
            {
                "en", new()
                {
                    { "play", "Play" },
                    { "settings", "Settings" },
                    { "quit", "Quit" },
                    { "quest_explore_town", "Explore the town" },
                    { "quest_find_item", "Find {0}" },
                    { "quest_return_to_npc", "Return to the npc" },
                    { "quest_buy_breakfast", "Buy breakfast" },
                    { "quest_talk_baker", "Talk to the baker" },

                    { "quest_serve_customers", "Serve the customers" },
                    { "quest_talk_customer", "Talk to a customer" },

                    { "quest_completed", "Completed!" },

                    { "settings_volume_label", "Volume" },
                    { "settings_native_language_label", "Native Language: " },
                    { "settings_foreign_language_label", "Foreign Language: " }
                }

            }
    };

    public string GetText(string key)
    {
        if (!_translations.ContainsKey(CurrentLanguage))
            return key;

        if (!_translations[CurrentLanguage].ContainsKey(key))
            return key;

        return _translations[CurrentLanguage][key];
    }

    public string Format(string key, params object[] args)
    {
        return string.Format((string)Get(key), args);
    }
}
