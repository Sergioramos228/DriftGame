using System;
using UnityEngine;

[Serializable]
public struct CarBuild
{
    public CarBuild(Color color, string name)
    {
        Color = color;
        Name = name;
    }

    public Color Color;
    public string Name;

    public string Serialize()
    {
        return JsonUtility.ToJson(this);
    }

    public static CarBuild Deserialize(string json)
    {
        return JsonUtility.FromJson<CarBuild>(json);
    }
}
