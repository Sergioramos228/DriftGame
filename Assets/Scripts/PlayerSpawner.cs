using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _camera;
    [SerializeField] private UIView _view;

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            GameObject player = PhotonNetwork.Instantiate(_prefab.name, transform.position, Quaternion.identity);
            CarPhysic carPhysic = player.GetComponentInChildren<CarPhysic>();
            DriftProcessor driftProcessor = player.GetComponentInChildren<DriftProcessor>();
            CarBooster booster = player.GetComponentInChildren<CarBooster>();
            CarBuildUnpacker unpacker = player.GetComponentInChildren<CarBuildUnpacker>();
            _view.ApplyCar(carPhysic, driftProcessor, booster);
            booster.Init();
            _camera.SetParent(player.GetComponentInChildren<CameraPosition>().Transform);
            _camera.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            string jsonCarBuild = (string)PhotonNetwork.CurrentRoom.CustomProperties[$"CarBuild_{PhotonNetwork.LocalPlayer.UserId}"];
            unpacker.ApplyCarBuild(jsonCarBuild);
        }
    }
}
