using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    [SerializeField] private Slider _red;
    [SerializeField] private Slider _green;
    [SerializeField] private Slider _blue;
    [SerializeField] private Image _shower;

    private Color _color;

    public Color Color => _color;

    private void Awake()
    {
        _red.onValueChanged.AddListener(OnColorChanged);
        _green.onValueChanged.AddListener(OnColorChanged);
        _blue.onValueChanged.AddListener(OnColorChanged);
        _color = _shower.color;
    }

    private void OnColorChanged (float call)
    {
        _color.r = _red.value;
        _color.g = _green.value;
        _color.b = _blue.value;

        _shower.color = _color;
    }
}
