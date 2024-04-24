using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    private Dictionary<TypesOfPlayerProperties, float> _properties;

    public string Name { get; private set; }
    public IDictionary<TypesOfPlayerProperties, float> Values => _properties;
    public event Action Changed;

    private void Awake()
    {
        LoadProperties();
    }

    public void SetProperty(TypesOfPlayerProperties property, float value)
    {
        if (_properties.ContainsKey(property))
            _properties[property] = value;
        else
            _properties.Add(property, value);

        PlayerPrefs.SetFloat(property.ToString(), value);
        Changed?.Invoke();
    }

    public void AddGold(float gold)
    {
        _properties[TypesOfPlayerProperties.Gold] += gold;
        PlayerPrefs.SetFloat(TypesOfPlayerProperties.Gold.ToString(), _properties[TypesOfPlayerProperties.Gold]);
        Changed?.Invoke();
    }

    public void ChangeName(string name)
    {
        Name = name;
        PlayerPrefs.SetString("Name", Name);
    }

    public void Initialize()
    {
        Changed?.Invoke();
    }

    private void LoadProperties()
    {
        _properties = new Dictionary<TypesOfPlayerProperties, float>();

        Name = PlayerPrefs.GetString("Name");

        foreach (TypesOfPlayerProperties property in Enum.GetValues(typeof(TypesOfPlayerProperties)))
            _properties.Add(property, PlayerPrefs.GetFloat(property.ToString(), 0));
    }
}