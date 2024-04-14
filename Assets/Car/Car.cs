using System;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private Transform _body;
    [SerializeField] private DriftProcessor _drift;
    [SerializeField] private NameView _nameView;

    public string Name => _nameView.Name;
    public float Drift { get; private set; }

    public Vector3 Body => _body.position;
    public event Action ValueChanged;

    private void OnEnable()
    {
        _drift.ScoreChange += OnScoreChanged;
    }

    private void OnDisable()
    {
        _drift.ScoreChange -= OnScoreChanged;
    }

    private void OnScoreChanged(float score)
    {
        Drift = score;
        ValueChanged?.Invoke();
    }
}
