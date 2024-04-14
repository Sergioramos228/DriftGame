using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class TraceLoader : MonoBehaviour
{
    [SerializeField] private List<Trace> _traces;
    [SerializeField] private CarSpawner _spawner;
    [SerializeField] private TraceView _view;

    private PhotonView _photonView;
    private Trace _current;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            int selectedTrace = (int)PhotonNetwork.CurrentRoom.CustomProperties["Trace"];
            _current = _traces[selectedTrace];
            TraceZone position = _current.GetStartPosition(PhotonNetwork.LocalPlayer.ActorNumber);
            Car car = _spawner.Load(position.Point, Quaternion.LookRotation(position.Forward));
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
