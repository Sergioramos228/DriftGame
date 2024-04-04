using UnityEngine;

public class WheelView : MonoBehaviour
{
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    public void SetWorld(Vector3 position, Quaternion angle)
    {
        _transform.rotation = angle;
        _transform.position = position;
    }
}
