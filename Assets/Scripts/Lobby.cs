using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private Menu _menu;
    [SerializeField] private AdsManager _ads;

    private void Awake()
    {
        _ads.LoadBanner();
        _menu.SetScreens(Menu.Screen.Connect);
    }

    public void OnClickConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
        _menu.SetScreens(Menu.Screen.Wait);
    }

    public override void OnConnected()
    {
        _ads.DestroyBanner();
        _menu.SetScreens(Menu.Screen.Garage);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _ads.DestroyBanner();
        _menu.SetScreens(Menu.Screen.Disconnect);
        Debug.Log($"Disconnected. Cause: {cause}");
    }
}
