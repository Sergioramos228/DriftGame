using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private Menu _menu;

    private void Awake()
    {
        _menu.SetScreens(Menu.Screen.Connect);
    }

    public void OnClickConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
        _menu.SetScreens(Menu.Screen.Wait);
    }

    public override void OnConnected()
    {
        _menu.SetScreens(Menu.Screen.Garage);
    }

    public override void OnJoinedLobby()
    {
        _menu.SetScreens(Menu.Screen.Room);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected. Cause: {cause}");
    }
}
