using UnityEngine;
using Photon.Pun;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _camera;
    [SerializeField] private UIView _view;

    public Car Load(Vector3 position, Quaternion forward, Race race)
    {
        GameObject player = PhotonNetwork.Instantiate(_prefab.name, position, forward);
        CarPhysic carPhysic = player.GetComponentInChildren<CarPhysic>();
        DriftProcessor driftProcessor = player.GetComponentInChildren<DriftProcessor>();
        CarBooster booster = player.GetComponentInChildren<CarBooster>();
        CarBuildUnpacker unpacker = player.GetComponentInChildren<CarBuildUnpacker>();
        Car car = player.GetComponentInChildren<Car>();
        car.ConnectToRace(race);
        _view.ApplyCar(carPhysic, driftProcessor, booster);
        booster.Init();
        _camera.SetParent(player.GetComponentInChildren<CameraPosition>().Transform);
        _camera.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        unpacker.ApplyCarBuild();
        return car;
    }
}
