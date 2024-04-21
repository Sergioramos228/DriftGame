using System;
using System.Collections.Generic;
using UnityEngine;

public class CarRacingTracker
{
    private Queue<RaceZone> _raceZones;
    private List<RaceZone> _fullWay;
    private RaceZone _currentZone;
    private Vector3 _vector;

    public CarRacingTracker(Car car, IEnumerable<RaceZone> way)
    {
        Car = car;
        CurrentPoint = 0;
        _raceZones = new Queue<RaceZone>();
        _fullWay = new List<RaceZone>();

        foreach (RaceZone raceZone in way)
        {
            _raceZones.Enqueue(raceZone);
            _fullWay.Add(raceZone);
        }

        SwitchNextZone();
    }

    public Car Car { get; private set; }

    public event Action<CarRacingTracker> Finished;

    public int CurrentPoint { get; private set; }

    public float DistanceToNextPoint()
    {
        return Vector3.Distance(Car.Body, _currentZone.Point);
    }

    public void Check()
    {
        Vector3 carVector = Car.Body - _currentZone.Point;

        if (Vector3.Dot(_vector, carVector.normalized) > 0 && DistanceToNextPoint() < _currentZone.WorkingDistance)
            SwitchNextZone();
    }

    private void Reset()
    {
        CurrentPoint = 0;
        _raceZones.Clear();

        foreach(RaceZone traceZone in _fullWay)
            _raceZones.Enqueue(traceZone);

        SwitchNextZone();
    }

    private void SwitchNextZone()
    {
        if (Car.IsMine)
            _currentZone?.Hide();

        if (_raceZones.Count == 0)
        {
            Reset();
            Finished?.Invoke(this);
            return;
        }

        else if (_raceZones.Count == 1)
        {
            _currentZone = _raceZones.Dequeue();
            _vector = _currentZone.Forward;
        }
        else
        {
            _currentZone = _raceZones.Dequeue();
            _vector = (_raceZones.Peek().Point - _currentZone.Point).normalized;
        }

        CurrentPoint++;

        if (Car.IsMine)
            _currentZone.Show();
    }
}
