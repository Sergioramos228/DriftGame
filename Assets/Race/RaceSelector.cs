using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceSelector : MonoBehaviour
{
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private TMP_Text _raceLabel;

    private int _lastTrace;

    public TracesTypes Current { get; private set; }

    private void Awake()
    {
        Current = 0;
        _lastTrace = Enum.GetValues(typeof(TracesTypes)).Length - 1;
        UpdateLabel();
    }

    private void OnEnable()
    {
        _previousButton.onClick.AddListener(SelectPrevious);
        _nextButton.onClick.AddListener(SelectNext);
    }

    private void OnDisable()
    {
        _previousButton.onClick.RemoveListener(SelectPrevious);
        _nextButton.onClick.RemoveListener(SelectNext);
    }

    private void SelectNext()
    {
        if (Current == (TracesTypes)_lastTrace)
            Current = 0;
        else
            Current += 1;

        UpdateLabel();
    }

    private void SelectPrevious()
    {
        if (Current == 0)
            Current = (TracesTypes)_lastTrace;
        else
            Current -= 1;

        UpdateLabel();
    }

    private void UpdateLabel()
    {
        _raceLabel.text = Current.ToString();
    }
}
