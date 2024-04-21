using System;
using TMPro;
using UnityEngine;

public class RaceLeaderboardPlayerView : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _points;
    [SerializeField] private CanvasGroup _visionController;

    private Car _car;

    private void OnEnable()
    {
        UpdateCarSubscription(true);
    }

    private void OnDisable()
    {
        UpdateCarSubscription(false);
    }

    public void Show(Car car)
    {
        UpdateCarSubscription(false);
        _car = car;
        UpdateCarSubscription(true);
        ShowCarValues();
    }

    public void ChangeVision(float newVision)
    {
        _visionController.alpha = newVision;
    }

    public void OnCarValueChanged()
    {
        ShowCarValues();
    }

    private void ShowCarValues()
    {
        _name.text = _car.Name;
        _points.text = Math.Round(_car.Drift, 1).ToString();
    }

    private void UpdateCarSubscription(bool isSubscribed)
    {
        if (_car == null)
            return;

        if (isSubscribed)
            _car.ValueChanged += OnCarValueChanged;
        else
            _car.ValueChanged -= OnCarValueChanged;
    }
}
