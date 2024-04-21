using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class Race : MonoBehaviour
{
    private const int SecondsInMinute = 60;
    private const int Second = 1;

    [SerializeField] private List<RaceZone> _way;
    [SerializeField] private int _countOfCircles;
    [SerializeField] private List<RaceZone> _startPositions;
    [SerializeField] private int _raceSeconds = 120;

    private List<Car> _cars;
    private RaceTracker _raceTracker;

    public int CountPlayers => _cars.Count;
    public int CountCircles => _countOfCircles;

    public event Action<int, int> TimeChanged;
    public event Action<IEnumerable<Car>> LeaderboardChanged;
    public event Action<int> CirclesChanged;
    public event Action<int> CountPlayerChanged;
    public event Action<int> WeFinished;
    public event Action HasExitTime;

    private void Awake()
    {
        _cars = new List<Car>();
        StartCoroutine(Timer());
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

    private IEnumerator Timer()
    {
        WaitForSeconds second = new WaitForSeconds(Second);
        int seconds;
        int minutes;
        yield return null;

        while (_raceSeconds > 0)
        {
            _raceSeconds -= Second;
            seconds = _raceSeconds % SecondsInMinute;
            minutes = (int)Math.Floor((decimal)_raceSeconds / SecondsInMinute);
            TimeChanged?.Invoke(minutes, seconds);
            yield return second;
        }

        HasExitTime?.Invoke();
    }

    public void Initialize(Car car)
    {
        _raceTracker = new RaceTracker(_way, _countOfCircles, car);
        _raceTracker.WeFinishedCircle += OnFinishCircle;
        _raceTracker.CarFinishedRace += OnCarFinish;
        _raceTracker.LeaderboardChanged += OnLeaderboardChanged;
        ApplyCar(car);
    }

    public void ApplyCar(Car car)
    {
        if (_cars.Contains(car))
            return;

        _cars.Add(car);
        _raceTracker.ApplyCar(car);
        LeaderboardChanged?.Invoke(_cars);
        CountPlayerChanged?.Invoke(_cars.Count);
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
