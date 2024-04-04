using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections;

public class MapLoader : MonoBehaviourPunCallbacks
{
    [SerializeField] private string _levelName = "Game";
    [SerializeField] private TMP_InputField _temp;
    [SerializeField] private ColorPicker _colorPicker;

    private PhotonView _photonView;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        _photonView = GetComponent<PhotonView>();
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;

        if(PhotonNetwork.IsMasterClient)
            LoadScene();
        else
            StartCoroutine(WaitForMasterClient());
    }

    private void LoadScene()
    {
        CarBuild build = new CarBuild(_colorPicker.Color, _temp.text);
        _photonView.RPC("SetProperties", RpcTarget.All, PhotonNetwork.LocalPlayer.UserId, build.Serialize());
        PhotonNetwork.LoadLevel(_levelName);
    }

    private IEnumerator WaitForMasterClient()
    {
        while (!PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }

        LoadScene();
    }

    [PunRPC]
    public void SetProperties(string actorGUID, string jsonDataBuild)
    {
        PhotonNetwork.CurrentRoom.CustomProperties.Add($"CarBuild_{actorGUID}", jsonDataBuild);
    }
}
