using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIPanelShower : MonoBehaviour
{
    [SerializeField] private Canvas _main;
    [SerializeField] private Canvas _win;
    [SerializeField] private Canvas _finish;
    [SerializeField] private Canvas _lose;
    [SerializeField] private Canvas _menu;
    [SerializeField] private Canvas _endButtons;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _returnGameButton;

    private List<Canvas> _panels;
    private List<Canvas> _finishPanels;

    private Race _race;

    private void Awake()
    {
        _panels = new List<Canvas>()
        {
            _main,
            _win,
            _finish,
            _lose,
            _menu
        };
        _finishPanels = new List<Canvas>()
        {
            _finish,
            _lose,
            _win
        };
    }

    private void OnEnable()
    {
        _menuButton.onClick.AddListener(OnMenuButtonClick);
        _returnGameButton.onClick.AddListener(OnPlayButtonClick);
    }

    private void OnDisable()
    {
        _menuButton.onClick.RemoveListener(OnMenuButtonClick);
        _returnGameButton.onClick.RemoveListener(OnPlayButtonClick);
    }

    public void ApplyRace(Race race)
    {
        if (_race != null)
            return;

        _race = race;
        race.WeFinished += OnFinishRace;
        _race.HasExitTime += OnExitTime;
    }

    private void OnExitTime()
    {
        ShowPanel(_lose);
    }

    private void OnFinishRace(int place)
    {
        if (place == 1)
            ShowPanel(_win);
        else if (place >= 4)
            ShowPanel(_lose);
        else
            ShowPanel(_finish);
    }

    private void OnMenuButtonClick()
    {
        ShowPanel(_menu);
    }

    private void OnPlayButtonClick()
    {
        ShowPanel(_main);
    }

    private void ShowPanel(Canvas panel)
    {
        foreach (Canvas item in _panels)
            item.enabled = item.Equals(panel);

        _endButtons.enabled = _finishPanels.Contains(panel);
    }
}
