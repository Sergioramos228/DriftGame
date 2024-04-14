using TMPro;
using UnityEngine;

public class NameView : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;

    public string Name { get; private set; }

    public void ApplyName(string name)
    {
        if (Name != null)
            return;

        _name.text = name;
        Name = name;
    }
}
