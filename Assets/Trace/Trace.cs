using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace : MonoBehaviour
{
    private const int SecondsInMinute = 60;
    private const int Second = 1;

    [SerializeField] private List<TraceZone> _way;
    [SerializeField] private int _countOfCircles;
    [SerializeField] private List<TraceZone> _startPositions;
    [SerializeField] private int _secondsToTrace = 120;

    private List<Car> _cars;
    private RaceTracker _raceTracker;

    public int CountPlayers => _cars.Count;
    public int CountCircles => _countOfCircles;
    public event Action<int, int> TimeChanged;
    public event Action<IEnumerable<Car>> LeaderboardChanged;
    public event Action<int> CirclesChanged;
    public event Action<int> CountPlayerChanged;

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
            _raceTracker.CarFinishedTrace -= OnCarFinishTrace;
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

        while (_secondsToTrace > 0)
        {
            _secondsToTrace -= Second;
            seconds = _secondsToTrace % SecondsInMinute;
            minutes = (int)Math.Floor((decimal)_secondsToTrace / SecondsInMinute);
            TimeChanged?.Invoke(minutes, seconds);
            yield return second;
        }
    }
    
    public void Initialize(Car car)
    {
        _raceTracker = new RaceTracker(_way, _countOfCircles, car);
        _raceTracker.WeFinishedCircle += OnFinishCircle;
        _raceTracker.CarFinishedTrace += OnCarFinishTrace;
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

    public TraceZone GetStartPosition(int number)
    {
        return _startPositions[number];
    }

    private void OnLeaderboardChanged(IEnumerable<Car> leaderboard)
    {
        LeaderboardChanged?.Invoke(leaderboard);
    }

    private void OnCarFinishTrace(Car car)
    {
        //to do
    }

    private void OnFinishCircle(int circle)
    {
        CirclesChanged?.Invoke(circle);
    }

}
