using System;
using System.Collections.Generic;
using System.Linq;

public class RaceTracker
{
    private List<TraceZone> _way = new List<TraceZone>();
    private List<CarRacingTracker> _observers = new List<CarRacingTracker>();
    private Dictionary<Car, int> _carsCircles = new Dictionary<Car, int>();
    private List<Car> _leaderboard = new List<Car>();

    private int _maxCircles;
    private Car _myCar;

    public RaceTracker(IEnumerable<TraceZone> way, int maxCircles, Car myCar)
    {
        foreach (TraceZone zone in way)
            _way.Add(zone);

        _maxCircles = maxCircles;
        _myCar = myCar;
    }

    public event Action<IEnumerable<Car>> LeaderboardChanged;
    public event Action<Car> CarFinishedTrace;
    public event Action<int> WeFinishedCircle;

    public void Check()
    {
        IEnumerable<CarRacingTracker> observers = _observers.ToArray();

        foreach (CarRacingTracker observer in observers)
            observer.Check();

        UpdateLeaderboard();
    }

    public void ApplyCar(Car car)
    {
        _carsCircles.Add(car, 0);
        AddCarToLeaderboard(car);
    }

    private void AddCarToLeaderboard(Car car)
    {
        CarRacingTracker observer = new CarRacingTracker(car, _way);
        _observers.Add(observer);
        observer.Finished += OnCarFinishedCircle;
        UpdateLeaderboard();
    }

    private void OnCarFinishedCircle(CarRacingTracker observer)
    {
        _carsCircles[observer.Car]++;

        if (observer.Car == _myCar)
            WeFinishedCircle?.Invoke(_carsCircles[observer.Car]);

        if (_carsCircles[observer.Car] == _maxCircles)
            CarFinishedTrace?.Invoke(observer.Car);
    }

    private void UpdateLeaderboard()
    {
        List<Car> cars = (from circles in _carsCircles
                          join observers in _observers on circles.Key equals observers.Car
                          select new { car = circles.Key, circle = circles.Value, point = observers.CurrentPoint, distance = observers.DistanceToNextPoint() })
                    .OrderByDescending(x => x.circle)
                    .ThenByDescending(x => x.point)
                    .ThenBy(x => x.distance)
                    .Select(x => x.car)
                    .ToList();

        if (_leaderboard.SequenceEqual(cars) == false)
        {
            _leaderboard = cars;
            LeaderboardChanged?.Invoke(cars);
        }
    }
}
