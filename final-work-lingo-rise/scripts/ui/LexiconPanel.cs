using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class LexiconPanel : Control
{
    [Export] private PackedScene _wordCardScene;
    [Export] private PackedScene _lockedCardScene;
    [Export] private GridContainer _grid;

    [Export] private Button _nextButton;
    [Export] private Button _previousButton;

    [Export] private Label _pageLabel;

    private LexiconManager _wordSystem;
    private LocalLexiconRepository _repository;

    private List<ILexiconEntry> _entries = new();

    private int _currentPage = 0;

    private const int CardsPerPage = 4;

    private Vector2 _touchStartPosition;
    private bool _isSwiping = false;

    private const float SwipeThreshold = 100f;

    public override void _Ready()
    {
        _wordSystem = GetNode<LexiconManager>("/root/LexiconManager");
        _repository = GetNode<LocalLexiconRepository>("/root/LocalLexiconRepository");

        _wordSystem.OnWordUnlocked += HandleWordUnlocked;

        _nextButton.Pressed += NextPage;
        _previousButton.Pressed += PreviousPage;

        RefreshEntries();
        PopulatePage();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touchEvent)
        {
            if (touchEvent.Pressed)
            {
                _touchStartPosition = touchEvent.Position;
                _isSwiping = true;
            }
            else
            {
                _isSwiping = false;
            }
        }

        if (@event is InputEventScreenDrag dragEvent && _isSwiping)
        {
            Vector2 swipeDelta = dragEvent.Position - _touchStartPosition;

            if (Mathf.Abs(swipeDelta.X) > SwipeThreshold)
            {
                if (swipeDelta.X < 0)
                {
                    NextPage();
                }
                else
                {
                    PreviousPage();
                }

                _isSwiping = false;
            }
        }
    }

    private void RefreshEntries()
    {
        _entries = _wordSystem
            .GetUnlockedWords()
            .Select(id => _repository.GetById(id))
            .Where(entry => entry != null)
            .ToList();
    }

    private void PopulatePage()
    {
        ClearGrid();

        int startIndex = _currentPage * CardsPerPage;

        var pageEntries = _entries
            .Skip(startIndex)
            .Take(CardsPerPage);

        foreach (var entry in pageEntries)
        {
            var card = _wordCardScene.Instantiate<WordCard>();

            card.Setup(entry);

            _grid.AddChild(card);
        }

        int displayedCards = pageEntries.Count();

        for (int i = displayedCards; i < CardsPerPage; i++)
        {
            var lockedCard = _lockedCardScene.Instantiate();

            _grid.AddChild(lockedCard);
        }

        UpdatePageLabel();
        UpdateButtons();
    }

    private void ClearGrid()
    {
        foreach (Node child in _grid.GetChildren())
        {
            child.QueueFree();
        }
    }

    private void NextPage()
    {
        int maxPage = Mathf.CeilToInt((float)_entries.Count / CardsPerPage) - 1;

        if (_currentPage >= maxPage)
            return;

        _currentPage++;

        PopulatePage();
    }

    private void PreviousPage()
    {
        if (_currentPage <= 0)
            return;

        _currentPage--;

        PopulatePage();
    }

    private void UpdatePageLabel()
    {
        int maxPage = Mathf.Max(
            1,
            Mathf.CeilToInt((float)_entries.Count / CardsPerPage)
        );

        _pageLabel.Text = $"Page {_currentPage + 1}/{maxPage}";
    }

    private void UpdateButtons()
    {
        int maxPage = Mathf.CeilToInt((float)_entries.Count / CardsPerPage) - 1;

        _previousButton.Disabled = _currentPage <= 0;
        _nextButton.Disabled = _currentPage >= maxPage;
    }

    private void HandleWordUnlocked(string wordId)
    {
        RefreshEntries();

        int maxPage = Mathf.CeilToInt((float)_entries.Count / CardsPerPage) - 1;

        if (_currentPage > maxPage)
        {
            _currentPage = maxPage;
        }

        PopulatePage();
    }
}