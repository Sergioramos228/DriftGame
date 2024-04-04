using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarComponentView : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _confirm;

    private Component _component;

    public event Action<Component, CarComponentView> SelectButtonClick;

    private void OnEnable()
    {
        _confirm.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _confirm.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        SelectButtonClick?.Invoke(_component, this);
    }

    public void Render(Component component)
    {
        _component = component;
        _label.text = component.Label;
        _icon.sprite = component.Icon;
    }
}