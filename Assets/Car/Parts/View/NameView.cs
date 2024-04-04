using TMPro;
using UnityEngine;

public class NameView : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;

    public void ApplyName(string name)
    {
        _name.text = name;
    }
}
