using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TraceLoader : MonoBehaviour
{
    [SerializeField] private List<Trace> _traces;
    [SerializeField] private CarSpawner _spawner;
    [SerializeField] private TraceView _view;

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            int selectedTrace = (int)PhotonNetwork.CurrentRoom.CustomProperties["Trace"];
            Trace current = _traces[selectedTrace];
            Vector3 position = current.GetStartPosition(PhotonNetwork.LocalPlayer.ActorNumber);
            Car car = _spawner.Load(position);
            current.ApplyCar(car);
            _view.ApplyTrace(current);
            current.UpdateInfo();
        }
    }
}
