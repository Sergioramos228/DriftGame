using TMPro;
using UnityEngine;

public class PlayerNicknameView : MonoBehaviour
{
    [SerializeField] private TMP_InputField _getter;
    [SerializeField] private PlayerProperties _properties;

    private void OnEnable()
    {
        _getter.onValueChanged.AddListener(OnNameChanged);
        _properties.Initialized += OnNameInitialized;
    }

    private void OnDisable()
    {
        _getter.onValueChanged.RemoveListener(OnNameChanged);
        _properties.Initialized -= OnNameInitialized;
    }

    private void OnNameChanged(string newName)
    {
        _properties.ChangeName(newName);
    }

    private void OnNameInitialized()
    {
        _getter.text = _properties.Name;
    }
}
