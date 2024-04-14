using UnityEngine;

public class TraceZone : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private MeshRenderer _view;
    [SerializeField] private int _prioritized = 0;

    public Vector3 Point => _transform.position;
    public Vector3 Forward => _transform.forward;

    public void Show()
    {
        _view.enabled = true;
    }

    public void Hide()
    {
        _view.enabled = false;
    }
}
