using System;
using System.Collections.Generic;
using UnityEngine;

public class GarageView : MonoBehaviour
{
    [SerializeField] private List<Component> _components;
    [SerializeField] private CarComponentView _carComponentView;
    [SerializeField] private GameObject _componentsContainer;

    private List<CarComponentView> _componentViews = new();

    private void Start()
    {
        for (int i = 0; i < _components.Count; i++)
            AddItem(_components[i]);
    }

    private void AddItem(Component weapon)
    {
        CarComponentView view = Instantiate(_carComponentView, _componentsContainer.transform);
        view.SelectButtonClick += OnSelectButtonClick;
        view.Render(weapon);
    }

    private void OnSelectButtonClick(Component component, CarComponentView view)
    {
        TrySelect(component, view);
    }

    private void TrySelect(Component component, CarComponentView view)
    {
        
    }
}
