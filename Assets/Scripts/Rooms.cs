using UnityEngine;
using Photon.Pun;

public class Rooms : MonoBehaviour
{
    public void QuickGame()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }    
}
