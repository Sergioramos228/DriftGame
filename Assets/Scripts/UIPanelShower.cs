using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIPanelShower : MonoBehaviour
{
    [SerializeField] private Canvas _main;
    [SerializeField] private Canvas _win;
    [SerializeField] private Canvas _finish;
    [SerializeField] private Canvas _lose;
    [SerializeField] private Canvas _menu;
    [SerializeField] private Canvas _start;
    [SerializeField] private Canvas _endButtons;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _returnGameButton;
    [SerializeField] private RaceTimer _timer;
    [SerializeField] private TMP_Text _loseLabel;

    private List<Canvas> _panels;
    private List<Canvas> _finishPanels;

    private Race _race;

    private void Awake()
    {
        _loseLabel.text = "Вы последний! Получено x0.5 очков дрифта.";
        _panels = new List<Canvas>()
        {
            _main,
            _win,
            _finish,
            _lose,
            _menu,
            _start
        };
        _finishPanels = new List<Canvas>()
        {
            _finish,
            _lose,
            _win
        };

        ShowPanel(_start);
    }

    private void OnEnable()
    {
        _menuButton.onClick.AddListener(OnMenuButtonClick);
        _returnGameButton.onClick.AddListener(OnPlayButtonClick);
        _timer.HasExitTime += OnExitTime;
    }

    private void OnDisable()
    {
        _menuButton.onClick.RemoveListener(OnMenuButtonClick);
        _returnGameButton.onClick.RemoveListener(OnPlayButtonClick);
        _timer.HasExitTime -= OnExitTime;
    }

    public void EnterRace(Race race)
    {
        if (_race != null)
            return;

        _race = race;
        race.WeFinished += OnFinishRace;
        ShowPanel(_main);
    }

    private void OnExitTime()
    {
        _loseLabel.text = "Вы не успели! Получено 0 очков дрифта.";
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

        _timer.HasExitTime -= OnExitTime;
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
