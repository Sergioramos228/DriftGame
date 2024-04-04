using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class CarBuildUnpacker : MonoBehaviourPun
{
    [SerializeField] private ColorSetter _colorSetter;
    [SerializeField] private NameView _nameView;

    private PhotonView _photonView;
    private string _myID;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        _myID = PhotonNetwork.LocalPlayer.UserId;
    }

    public void ApplyCarBuild(string jsonCarBuild)
    {
        SetJsonCarBuild(jsonCarBuild);
        _photonView.RPC("SendBuild", RpcTarget.AllBuffered, jsonCarBuild, _myID);
    }

    [PunRPC]
    public void SendBuild(string jsonCarBuild, string id)
    {
        if (id == _myID)
            SetJsonCarBuild(jsonCarBuild);
    }

    private void SetJsonCarBuild(string jsonCarBuild)
    {
        CarBuild carBuild = JsonUtility.FromJson<CarBuild>(jsonCarBuild);
        _nameView.ApplyName(carBuild.Name);
        _colorSetter.ApplyColor(carBuild.Color);
    }
}