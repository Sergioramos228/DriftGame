using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject[] _menus;
    [SerializeField] private PlayerProperties _playerProperties;

    public void SetScreens(Screen screen)
    {
        for (int i = 0; i < _menus.Length; i++)
            _menus[i].SetActive(i == (int)screen);

        if (screen == Screen.Garage)
            _playerProperties.Initialize();
    }

    public enum Screen
    {
        Connect,
        Wait,
        Garage,
    }
}
