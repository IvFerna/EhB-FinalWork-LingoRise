using Godot;
using System;

public partial class QuestUI : Control
{
    public static QuestUI Instance;
    public Label _mainLabel;
    public Label _subLabel;

    private string _currentMainQuestKey;
    private string _currentSubQuestKey;
    public override void _Ready()
    {
        Instance = this;
        _mainLabel = GetNode<Label>("Panel/MainQuestLabel");
        _subLabel = GetNode<Label>("Panel/VBoxContainer/HBoxContainer/SubQuestLabel");

        var requestSystem =
            GetNode<RequestSystem>("/root/RequestSystem");

        requestSystem.RequestStateChanged += UpdateSubQuest;
        requestSystem.RequestCompleted += ClearSubQuest;

        _mainLabel.Text = LanguageManager.Instance.GetText("quest_explore_town");
        _subLabel.Text = "";
        _subLabel.Visible = false;

        LanguageManager.Instance.LanguageChanged += RefreshTexts;
    }

    private void UpdateSubQuest(RequestSystem.RequestState state, InventoryItem item)
    {
        _subLabel.Visible = true;
        switch (state)
        {
            case RequestSystem.RequestState.FindItem:

                _subLabel.Text =
                    $"Find {item.VocabularyEntry.ForeignWord}";
                _subLabel.Text = LanguageManager.Instance.Format(
                    "quest_find_item",
                    item.VocabularyEntry.ForeignWord
                    );
                break;

            case RequestSystem.RequestState.ReturnToNpc:
                _subLabel.Text =
                    "Return to the npc";
                _subLabel.Text = (string)LanguageManager.Instance.GetText("quest_return_to_npc");
                break;
            case RequestSystem.RequestState.Completed:
                _subLabel.Text = null;
                _subLabel.Visible = false;
                break;


        }
    }

    private void ClearSubQuest()
    {
        _subLabel.Text = "Purchase complete";
    }

    public void SetQuest(string mainKey, string subKey = null)
    {
        _currentMainQuestKey = mainKey;
        _currentSubQuestKey = subKey;

        if (string.IsNullOrEmpty(subKey))
        {
            _subLabel.Visible = false;
            return;
        }

        RefreshTexts();
    }

    private void RefreshTexts()
    {
        if (!string.IsNullOrEmpty(_currentMainQuestKey))
            _mainLabel.Text = LanguageManager.Instance.GetText(_currentMainQuestKey);

        if (!string.IsNullOrEmpty(_currentSubQuestKey))
            _subLabel.Text = LanguageManager.Instance.GetText(_currentSubQuestKey);
    }
}
