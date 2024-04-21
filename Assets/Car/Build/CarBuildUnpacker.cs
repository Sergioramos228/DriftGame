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
        float coloR = (float)_photonView.Owner.CustomProperties["ColorR"];
        float coloG = (float)_photonView.Owner.CustomProperties["ColorG"];
        float coloB = (float)_photonView.Owner.CustomProperties["ColorB"];
        _colorSetter.ApplyColor(new Color(coloR, coloG, coloB));
    }
}