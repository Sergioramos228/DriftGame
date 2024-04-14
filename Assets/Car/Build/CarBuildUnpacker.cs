using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class CarBuildUnpacker : MonoBehaviourPun
{
    [SerializeField] private ColorSetter _colorSetter;
    [SerializeField] private NameView _nameView;

    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public void ApplyCarBuild()
    {
        if (_photonView.IsMine)
            _photonView.RPC("SendBuild", RpcTarget.AllBuffered);

        SetCarBuild();
    }

    [PunRPC]
    public void SendBuild()
    {
        SetCarBuild();
    }

    private void SetCarBuild()
    {
        _nameView.ApplyName(_photonView.Owner.NickName);
        CarBuild carBuild = (CarBuild)_photonView.Owner.CustomProperties["Settings"];
        _colorSetter.ApplyColor(carBuild.Color);
    }
}