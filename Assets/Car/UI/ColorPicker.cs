using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    private const float FillRectSameColor = 0.3f;

    [SerializeField] private Slider _red;
    [SerializeField] private Slider _green;
    [SerializeField] private Slider _blue;
    [SerializeField] private Image _shower;
    [SerializeField] private MeshRenderer _showCase;
    [SerializeField] private PlayerProperties _properties;

    private Dictionary<Slider, Image> _fillRects;
    private Color _color;

    public Color Color => _color;

    private void Awake()
    {
        _fillRects = new Dictionary<Slider, Image>
        {
            { _red, _red.fillRect.GetComponent<Image>() },
            { _green, _green.fillRect.GetComponent<Image>() },
            { _blue, _blue.fillRect.GetComponent<Image>() }
        };

        _red.onValueChanged.AddListener(OnColorChanged);
        _green.onValueChanged.AddListener(OnColorChanged);
        _blue.onValueChanged.AddListener(OnColorChanged);
        _properties.Initialized += OnInitialized;
        _color = _shower.color;
        ShowCurrentColor();
    }

    private void OnColorChanged(float call)
    {
        _color.r = _red.value;
        _color.g = _green.value;
        _color.b = _blue.value;
        ShowCurrentColor();
        _properties.SetProperty(TypesOfPlayerProperties.ColorR, _red.value);
        _properties.SetProperty(TypesOfPlayerProperties.ColorG, _green.value);
        _properties.SetProperty(TypesOfPlayerProperties.ColorB, _blue.value);
    }

    private void ShowCurrentColor()
    {
        _red.value = _color.r;
        ChangeFillColor(_red, _color.r, _color.r * FillRectSameColor, _color.r * FillRectSameColor);
        _green.value = _color.g;
        ChangeFillColor(_green, _color.g * FillRectSameColor, _color.g, _color.g * FillRectSameColor);
        _blue.value = _color.b;
        ChangeFillColor(_blue, _color.b * FillRectSameColor, _color.b * FillRectSameColor, _color.b);
        _shower.color = _color;
        _showCase.material.color = _color;
    }

    private void OnInitialized()
    {
        _color.r = _properties.Values[TypesOfPlayerProperties.ColorR];
        _color.g = _properties.Values[TypesOfPlayerProperties.ColorG];
        _color.b = _properties.Values[TypesOfPlayerProperties.ColorB];
        ShowCurrentColor();
    }

    private void ChangeFillColor(Slider slider, float r, float g, float b)
    {
        _fillRects[slider].color = new Color(r, g, b);
    }
}
