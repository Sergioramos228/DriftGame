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

    private void OnEnable()
    {
        _fillRects = new Dictionary<Slider, Image>
        {
            { _red, _red.fillRect.GetComponent<Image>() },
            { _green, _green.fillRect.GetComponent<Image>() },
            { _blue, _blue.fillRect.GetComponent<Image>() }
        };

        _red.onValueChanged.AddListener(OnColorRChanged);
        _green.onValueChanged.AddListener(OnColorGChanged);
        _blue.onValueChanged.AddListener(OnColorBChanged);
        _properties.Changed += OnInitialized;
        _color = _shower.color;
    }

    private void OnDisable()
    {
        _red.onValueChanged.RemoveListener(OnColorRChanged);
        _green.onValueChanged.RemoveListener(OnColorGChanged);
        _blue.onValueChanged.RemoveListener(OnColorBChanged);
    }

    private void OnColorRChanged(float redColor)
    {
        _color.r = redColor;
        _properties.SetProperty(TypesOfPlayerProperties.ColorR, redColor);
        ShowCurrentColor();
    }
    private void OnColorGChanged(float greenColor)
    {
        _color.g = greenColor;
        _properties.SetProperty(TypesOfPlayerProperties.ColorG, greenColor);
        ShowCurrentColor();
    }
    private void OnColorBChanged(float blueColor)
    {
        _color.b = blueColor;
        _properties.SetProperty(TypesOfPlayerProperties.ColorB, blueColor);
        ShowCurrentColor();
    }

    private void ShowCurrentColor()
    {
        ChangeFillColor(_red, _color.r, _color.r * FillRectSameColor, _color.r * FillRectSameColor);
        ChangeFillColor(_green, _color.g * FillRectSameColor, _color.g, _color.g * FillRectSameColor);
        ChangeFillColor(_blue, _color.b * FillRectSameColor, _color.b * FillRectSameColor, _color.b);
        _shower.color = _color;
        _showCase.material.color = _color;
    }

    private void OnInitialized()
    {
        _color.r = _properties.Values[TypesOfPlayerProperties.ColorR];
        _red.value = _color.r;
        _color.g = _properties.Values[TypesOfPlayerProperties.ColorG];
        _green.value = _color.g;
        _color.b = _properties.Values[TypesOfPlayerProperties.ColorB];
        _blue.value = _color.b;
        ShowCurrentColor();
    }

    private void ChangeFillColor(Slider slider, float r, float g, float b)
    {
        _fillRects[slider].color = new Color(r, g, b);
    }
}
