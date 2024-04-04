using TMPro;
using UnityEngine;
using System;

public class UIView : MonoBehaviour
{
    [SerializeField] private TMP_Text _speedometerView;
    [SerializeField] private TMP_Text _driftPointView;
    [SerializeField] private TMP_Text _boostPointView;

    private CarPhysic _car;
    private DriftProcessor _driftProcessor;
    private CarBooster _booster;

    private void Update()
    {
        if (_car != null)
            _speedometerView.text = GetFormatString(_car.Speed * 10, 3);
    }

    private void OnEnable()
    {
        if (_driftProcessor != null)
            _driftProcessor.ScoreChange += OnScoreChanged;

        if (_booster != null)
            _booster.BoosterCountChanged += OnBoosterCountChanged;
    }

    private void OnDisable()
    {
        if (_driftProcessor != null)
            _driftProcessor.ScoreChange -= OnScoreChanged;

        if (_booster != null)
            _booster.BoosterCountChanged -= OnBoosterCountChanged;
    }

    private void OnScoreChanged(float value)
    {
        _driftPointView.text = Math.Round(value, 1).ToString();
    }

    private void OnBoosterCountChanged(int count)
    {
        _boostPointView.text = count.ToString();
    }

    public void ApplyCar(CarPhysic car, DriftProcessor driftProcessor, CarBooster booster)
    {
        _driftProcessor = driftProcessor;
        _driftProcessor.ScoreChange += OnScoreChanged;
        _booster = booster;
        _booster.BoosterCountChanged += OnBoosterCountChanged;
        _car = car;
    }

    private string GetFormatString(float value, int count)
    {
        string result = ((int)value).ToString();

        if(result.Length > count)
            return result.Substring(0, 3);
        else
            return result;
    }
}
