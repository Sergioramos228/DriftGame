using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject[] _menus;
    [SerializeField] private PlayerProperties _playerProperties;
    [SerializeField] private List<Light> _lights;

    public void SetScreens(Screen screen)
    {
        for (int i = 0; i < _menus.Length; i++)
            _menus[i].SetActive(i == (int)screen);

        if (screen == Screen.Garage)
        {
            foreach (Light light in _lights)
                light.enabled = true;

            _playerProperties.Initialize();
        }
    }

    public enum Screen
    {
        Connect,
        Disconnect,
        Wait,
        Garage,
    }
}
