using UnityEngine;

public struct CarBuild
{
    public const int BytesForColor = 12;
    public Color Color;

    public CarBuild(Color color)
    {
        Color = color;
    }

    public CarBuild(byte[] bytes)
    {
        byte[] color = new byte[BytesForColor];

        for (int i = 0; i < BytesForColor; i++)
        {
            color[i] = bytes[i];
        }

        Color = (Color)PUNSerializationService.DeserializeColor(color);
    }

    public byte[] ConvertForMessage()
    {
        byte[] color = PUNSerializationService.SerializeColor(Color);

        byte[] result = new byte[BytesForColor];
        color.CopyTo(result, 0);
        return result;
    }
}
