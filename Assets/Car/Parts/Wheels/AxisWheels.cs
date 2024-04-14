using UnityEngine;

public class AxisWheels : MonoBehaviour
{
    private const float AckermanFactor = 0.74f;
    private const float MaxAngle = 62;

    [SerializeField] private Wheel _left;
    [SerializeField] private Wheel _right;

    public void SetSidewaySlip(float value = -1)
    {
        _left.SetSidewaySlip(value);
        _right.SetSidewaySlip(value);
    }

    public void ApplyMotorTorque(float torque)
    {
        _left.SetMotorTorque(torque);
        _right.SetMotorTorque(torque);
    }

    public void ApplyBrakeTorque(float torque)
    {
        _left.SetBrakeTorque(torque);
        _right.SetBrakeTorque(torque);
    }

    public void ApplyAngle(float angle)
    {
        float rightCoefficient = 1;
        float leftCoefficient = 1;

        if (angle > 0)
            leftCoefficient = AckermanFactor;
        else
            rightCoefficient = AckermanFactor;

        _left.SetAngle(angle * MaxAngle * leftCoefficient);
        _right.SetAngle(angle * MaxAngle * rightCoefficient);
    }
}
