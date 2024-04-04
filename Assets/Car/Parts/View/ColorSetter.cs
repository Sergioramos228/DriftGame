using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;

    public void ApplyColor(Color color)
    {
        _renderer.material.color = color;
    }
}