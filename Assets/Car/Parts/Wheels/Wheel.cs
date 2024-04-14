using TMPro;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private WheelCollider _collider;
    [SerializeField] private WheelView _view;

    private Vector3 _worldPosition;
    private Quaternion _worldRotation;
    private WheelFrictionCurve _sideway;
    private float _stiffness;

    private void Awake()
    {
        _sideway = _collider.sidewaysFriction;
        _stiffness = _sideway.stiffness;
    }

    public void SetAngle(float value)
    {
        _collider.steerAngle = value;
    }

    public void SetMotorTorque(float value)
    {
        _collider.motorTorque = value;
    }

    public void SetBrakeTorque(float value)
    {
        _collider.brakeTorque = value;
    }

    public void SetSidewaySlip(float value = -1)
    {
        if (value <= 0)
            _sideway.stiffness = _stiffness;
        else
            _sideway.stiffness = value;

        _collider.sidewaysFriction = _sideway;
    }

    private void Update()
    {
        _collider.GetWorldPose(out _worldPosition, out _worldRotation);
        _view.SetWorld(_worldPosition, _worldRotation);
    }
}
