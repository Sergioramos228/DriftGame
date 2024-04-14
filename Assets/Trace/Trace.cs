using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace : MonoBehaviour
{
    private const int SecondsInMinute = 60;
    private const int Second = 1;

    [SerializeField] private List<TraceZone> _way;
    [SerializeField] private int _countOfCircle;
    [SerializeField] private List<TraceZone> _startPositions;
    [SerializeField] private int _secondsToTrace = 120;

    private Dictionary<Car, int> _carCircles;
    private List<Car> _cars;
    private List<CarTracingObserver> _observers;

    public int CountPlayers => _cars.Count;
    public int CountCircles => _countOfCircle;
    public event Action<int, int> TimeChanged;
    public event Action<IEnumerable<Car>> LeaderboardChanged;
    public event Action<int> CirclesChanged;

    private void Awake()
    {
        _cars = new List<Car>();
        _observers = new List<CarTracingObserver>();
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        WaitForSeconds second = new WaitForSeconds(Second);
        int seconds = 0;
        int minutes = 0;
        yield return null;

        while(_secondsToTrace > 0)
        {
            _secondsToTrace -= Second;
            seconds = _secondsToTrace % SecondsInMinute;
            minutes = (int)Math.Floor((decimal)_secondsToTrace / SecondsInMinute);
            TimeChanged?.Invoke(minutes, seconds);
            yield return second;
        }
    }

    public void ApplyCar(Car car)
    {
        _cars.Add(car);
        _observers.Add(new CarTracingObserver(car));
        LeaderboardChanged?.Invoke(_cars);
    }

    public void UpdateInfo()
    {
        LeaderboardChanged?.Invoke(_cars);
    }

    public Vector3 GetStartPosition(int number)
    {
        return _startPositions[number].Point;
    }

}
