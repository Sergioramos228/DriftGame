using System;
using System.Collections.Generic;
using UnityEngine;

public class CarRacingTracker
{
    private Queue<TraceZone> _traceZones;
    private List<TraceZone> _fullWay;
    private TraceZone _currentZone;
    private Vector3 _vector;

    public CarRacingTracker(Car car, IEnumerable<TraceZone> way)
    {
        Car = car;
        CurrentPoint = 0;
        _traceZones = new Queue<TraceZone>();
        _fullWay = new List<TraceZone>();

        foreach (TraceZone traceZone in way)
        {
            _traceZones.Enqueue(traceZone);
            _fullWay.Add(traceZone);
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
        _traceZones.Clear();

        foreach(TraceZone traceZone in _fullWay)
            _traceZones.Enqueue(traceZone);

        SwitchNextZone();
    }

    private void SwitchNextZone()
    {
        if (Car.IsMine)
            _currentZone?.Hide();

        if (_traceZones.Count == 0)
        {
            Reset();
            Finished?.Invoke(this);
            return;
        }

        else if (_traceZones.Count == 1)
        {
            _currentZone = _traceZones.Dequeue();
            _vector = _currentZone.Forward;
        }
        else
        {
            _currentZone = _traceZones.Dequeue();
            _vector = (_traceZones.Peek().Point - _currentZone.Point).normalized;
        }

        CurrentPoint++;

        if (Car.IsMine)
            _currentZone.Show();
    }
}
