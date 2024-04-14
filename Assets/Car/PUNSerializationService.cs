using ExitGames.Client.Photon;
using UnityEngine;

public class PUNSerializationService
{
    private const float ByteCoefficient = 255;
    private const float FloatCoefficient = 1000f;

    public static void Register()
    {
        PhotonPeer.RegisterType(typeof(Color), 50, SerializeColor, DeserializeColor);
        PhotonPeer.RegisterType(typeof(CarBuild), 51, SerializeCarBuild, DeserializeCarBuild);
    }

    public static byte[] SerializeColor(object colorObject)
    {
        Color color = (Color)colorObject;
        byte[] bytes = new byte[4];
        bytes[0] = (byte)(color.r * ByteCoefficient);
        bytes[1] = (byte)(color.g * ByteCoefficient);
        bytes[2] = (byte)(color.b * ByteCoefficient);
        bytes[3] = (byte)(color.a * ByteCoefficient);

        return bytes;
    }

    public static object DeserializeColor(byte[] bytes)
    {
        Color color = new Color();
        color.r = (float)(bytes[0] / ByteCoefficient);
        color.g = (float)(bytes[1] / ByteCoefficient);
        color.b = (float)(bytes[2] / ByteCoefficient);
        color.a = (float)(bytes[3] / ByteCoefficient);

        return color;
    }

    public static byte[] SerializeCarBuild(object obj)
    {
        CarBuild carBuild = (CarBuild)obj;
        return carBuild.ConvertForMessage();
    }

    public static object DeserializeCarBuild(byte[] bytes)
    {
        return new CarBuild(bytes);
    }
}
