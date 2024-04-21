using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class RaceLoader : MonoBehaviour
{
    [SerializeField] private List<Race> _races;
    [SerializeField] private CarSpawner _spawner;
    [SerializeField] private RaceView _view;
    [SerializeField] private UIPanelShower _shower;

    private PhotonView _photonView;
    private Race _current;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public void Load()
    {
        if (PhotonNetwork.IsConnected)
        {
            int selectedRace = (int)PhotonNetwork.CurrentRoom.CustomProperties["Trace"];
            _current = _races[selectedRace];
            _shower.ApplyRace(_current);
            RaceZone position = _current.GetStartPosition(PhotonNetwork.LocalPlayer.ActorNumber);
            Car car = _spawner.Load(position.Point, Quaternion.LookRotation(position.Forward), _current);
            _current.Initialize(car);
            _view.ApplyTrace(_current);
            _current.UpdateInfo();
            _photonView.RPC("ConnectCar", RpcTarget.AllBuffered, car.MyId);
        }
    }

    [PunRPC]
    private void ConnectCar(int viewId)
    {
        PhotonView photonView = PhotonView.Find(viewId);
        _current.ApplyCar(photonView.GetComponent<Car>());
    }
}
