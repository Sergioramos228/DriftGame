using UnityEngine;

public abstract class Component : MonoBehaviour
{
    [SerializeField] private string _label;
    [SerializeField] private Sprite _icon;

    private bool _isSelected;

    public string Label => _label;
    public Sprite Icon => _icon;

    private void Awake()
    {
        _isSelected = false;
    }

    public void Select()
    {
        _isSelected = true;
    }
}
