using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

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
            int selectedRace = (int)PhotonNetwork.CurrentRoom.CustomProperties["Race"];
            _current = _races[selectedRace];
            _shower.EnterRace(_current);
            var sortedPlayers = PhotonNetwork.CurrentRoom.Players.OrderBy(c => c.Key).Select(c => c.Value);
            int myPosition = 0;

            foreach (var player in sortedPlayers)
            {
                if (player == PhotonNetwork.LocalPlayer)
                    break;

                myPosition++;
            }

            RaceZone position = _current.GetStartPosition(myPosition);
            Car car = _spawner.Load(position.Point, Quaternion.LookRotation(position.Forward), _current);
            _current.Initialize(car);
            _view.ApplyRace(_current);
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
