using UnityEngine;

public class CarPhysic : MonoBehaviour
{
    [SerializeField] private AxisWheels _frontWheels;
    [SerializeField] private AxisWheels _backWheels;
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private Transform _centerOfDriftMass;
    [SerializeField] private float _torque;
    [SerializeField] private float _maxBrakeTorque;
    [SerializeField, Range(0, 1)] private float _forwardBreakFactor;
    [SerializeField, Range(0, 1)] private float _backBreakFactor;
    [SerializeField] private Rigidbody _car;
    
    private float _currentBreakFactor;
    private Transform _transform;
    public float SqrSpeed => _car.velocity.sqrMagnitude;
    public float Speed => _car.velocity.magnitude;

    private void Awake()
    {
        _currentBreakFactor = _backBreakFactor;
        _car.centerOfMass = _centerOfMass.localPosition;
        _transform = _car.transform;
    }

    public void BreakTorque(float coefficient = 0)
    {
        _frontWheels.ApplyBrakeTorque(coefficient * _forwardBreakFactor * _maxBrakeTorque);
        _backWheels.ApplyBrakeTorque(coefficient * _currentBreakFactor * _maxBrakeTorque);
    }

    public void ApplyDirection(float direction)
    {
        _frontWheels.ApplyAngle(direction);
    }

    public void Boost(float power)
    {
        _car.AddForce(_transform.forward * power, ForceMode.Acceleration);
    }

    public void GasTorque(float coefficient = 0)
    {
        _backWheels.ApplyMotorTorque(coefficient * _torque);
        _frontWheels.ApplyMotorTorque(coefficient * _torque);
    }

    public void EnterDrift(float slipPower)
    {
        _currentBreakFactor = 0;
        _car.centerOfMass = _centerOfDriftMass.localPosition;
        _backWheels.SetSidewaySlip(slipPower);
    }

    public void ExitDrift()
    {
        _currentBreakFactor = _backBreakFactor;
        _car.centerOfMass = _centerOfMass.localPosition;
        _backWheels.SetSidewaySlip();
    }

    public float CalculateDriftAngle()
    {
        return Vector3.Angle(_car.velocity, _car.rotation * Vector3.forward);
    }
}
