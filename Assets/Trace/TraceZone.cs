using UnityEngine;

public class TraceZone : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private MeshRenderer _view;
    [SerializeField] private float _workingDistance = 3f;

    public Vector3 Point => _transform.position;
    public Vector3 Forward => _transform.forward;
    public float WorkingDistance => _workingDistance;

    private void Awake()
    {
        Hide();
    }

    public void Show()
    {
        if (_view != null)
            _view.enabled = true;
    }

    public void Hide()
    {
        if (_view != null)
            _view.enabled = false;
    }
}
