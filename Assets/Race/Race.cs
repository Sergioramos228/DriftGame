using Photon.Pun;
using Photon.Realtime;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Race : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<RaceZone> _way;
    [SerializeField] private int _countOfCircles;
    [SerializeField] private List<RaceZone> _startPositions;

    private List<Car> _cars;
    private RaceTracker _raceTracker;

    public int CountPlayers => _cars.Count;
    public int CountCircles => _countOfCircles;

    public event Action<IEnumerable<Car>> LeaderboardChanged;
    public event Action<int> CirclesChanged;
    public event Action<int> CountPlayerChanged;
    public event Action<int> WeFinished;

    private void Awake()
    {
        _cars = new List<Car>();
    }

    private void OnDestroy()
    {
        if (_raceTracker != null)
        {
            _raceTracker.WeFinishedCircle -= OnFinishCircle;
            _raceTracker.CarFinishedRace -= OnCarFinish;
            _raceTracker.LeaderboardChanged -= OnLeaderboardChanged;
        }
    }

    private void FixedUpdate()
    {
        if (_raceTracker != null)
            _raceTracker.Check();
    }

    public void Initialize(Car car)
    {
        _raceTracker = new RaceTracker(_way, _countOfCircles, car);
        _raceTracker.WeFinishedCircle += OnFinishCircle;
        _raceTracker.CarFinishedRace += OnCarFinish;
        _raceTracker.LeaderboardChanged += OnLeaderboardChanged;
        ApplyCar(car);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Car car = _cars.Where(car => car.Owner == otherPlayer).FirstOrDefault();

        if (car == null)
            return;

        _cars.Remove(car);
        _raceTracker.RemoveCar(car);
        CountPlayerChanged?.Invoke(_cars.Count);
        LeaderboardChanged?.Invoke(_cars);
    }

    public void ApplyCar(Car car)
    {
        if (_cars.Contains(car))
            return;

        _cars.Add(car);
        _raceTracker.ApplyCar(car);
        CountPlayerChanged?.Invoke(_cars.Count);
        LeaderboardChanged?.Invoke(_cars);
    }

    public void UpdateInfo()
    {
        LeaderboardChanged?.Invoke(_cars);
    }

    public RaceZone GetStartPosition(int number)
    {
        return _startPositions[number];
    }

    private void OnLeaderboardChanged(IEnumerable<Car> leaderboard)
    {
        LeaderboardChanged?.Invoke(leaderboard);
    }

    private void OnCarFinish(Car car)
    {
        if (car.IsMine)
        {
            int ourPosition = _raceTracker.GetCarPosition(car);
            WeFinished?.Invoke(ourPosition);
        }
    }

    private void OnFinishCircle(int circle)
    {
        CirclesChanged?.Invoke(circle);
    }
}
