using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject[] _menus;

    public void SetScreens(Screen screen)
    {
        for (int i = 0; i < _menus.Length; i++)
        {
            _menus[i].SetActive(i == (int)screen);
        }
    }

    public enum Screen
    {
        Connect = 0,
        Wait,
        Garage,
    }
}
