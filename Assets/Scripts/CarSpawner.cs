using UnityEngine;
using Photon.Pun;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _camera;
    [SerializeField] private UIView _view;
    [SerializeField] private PlayerProperties _properties;
    [SerializeField] private RaceTimer _timer;

    private Car _car;

    public Car Load(Vector3 position, Quaternion forward, Race race)
    {
        GameObject player = PhotonNetwork.Instantiate(_prefab.name, position, forward);
        CarPhysic carPhysic = player.GetComponentInChildren<CarPhysic>();
        carPhysic.SetUpgrade(_properties.Values[TypesOfPlayerProperties.EngineUpgrade], _properties.Values[TypesOfPlayerProperties.WheelUpgrade], _properties.Values[TypesOfPlayerProperties.SuspensionUpgrade]);
        DriftProcessor driftProcessor = player.GetComponentInChildren<DriftProcessor>();
        CarBooster booster = player.GetComponentInChildren<CarBooster>();
        CarBuildUnpacker unpacker = player.GetComponentInChildren<CarBuildUnpacker>();
        _car = player.GetComponentInChildren<Car>();
        _car.ConnectToRace(race, _timer);
        _timer.Countdown();
        _view.ApplyCar(carPhysic, driftProcessor, booster);
        booster.Init();
        _camera.SetParent(player.GetComponentInChildren<CameraPosition>().Transform);
        _camera.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        unpacker.ApplyCarBuild();
        _car.Finished += OnCarFinished;
        return _car;
    }

    private void OnCarFinished(float gold)
    {
        _properties.SetProperty(TypesOfPlayerProperties.Gold, _properties.Values[TypesOfPlayerProperties.Gold] + gold);
        _car.Finished -= OnCarFinished;
    }
}
