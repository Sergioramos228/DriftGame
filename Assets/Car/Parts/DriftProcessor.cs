using System;
using UnityEngine;

public class DriftProcessor : MonoBehaviour
{
    private const float MinAngleToScore = 30;
    private const float MaxAngleToScore = 110;
    private const float MinSpeedToScore = 30;

    [SerializeField] private CarPhysic _physic;
    [SerializeField] private float _driftBackWeelsStiffnes;
    [SerializeField] private float _driftSensitivityAngle = 4.8f;

    private bool _isInDrift;
    private float _angle;
    private float _score;

    public event Action<float> ScoreChange;

    private void FixedUpdate()
    {
        _angle = _physic.CalculateDriftAngle();
        UpdateScore();
        UpdateState();
    }

    private void UpdateScore()
    {
        if (_angle > MinAngleToScore && _angle < MaxAngleToScore && _physic.SqrSpeed > MinSpeedToScore)
        {
            _score += Time.deltaTime;
            ScoreChange?.Invoke(_score);
        }
    }

    private void UpdateState()
    {
        if (_angle > _driftSensitivityAngle && _isInDrift == false)
        {
            _physic.EnterDrift(_driftBackWeelsStiffnes);
            _isInDrift = true;
        }
        else if (_angle < _driftSensitivityAngle && _isInDrift)
        {
            _isInDrift = false;
            _physic.ExitDrift();
        }
    }
}
