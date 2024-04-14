using System;
using System.Collections.Generic;
using UnityEngine;

public class CarTracingObserver
{
    private Queue<TraceZone> _traceZones;
    private Car _car;
    private TraceZone _currentZone;
    private Vector3 _vector;

    public CarTracingObserver(Car car)
    {
        _car = car;
    }

    public event Action<Car> Finished;

    public void Check()
    {
        Vector3 carVector = _car.Body - _currentZone.Point;

        if (Vector3.Dot(_vector, carVector.normalized) > 0)
            SwitchNextZone();
    }

    private void SwitchNextZone()
    {
        if (_traceZones.Count == 0)
        {
            Finished?.Invoke(_car);
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
    }
}
