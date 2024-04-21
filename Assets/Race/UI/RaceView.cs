using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RaceView : MonoBehaviour
{
    [SerializeField] private RaceLeaderboard _leaderboard;
    [SerializeField] private TMP_Text _timerMinutes;
    [SerializeField] private TMP_Text _timerSeconds;
    [SerializeField] private TMP_Text _circlesCurrent;
    [SerializeField] private TMP_Text _circlesMaximum;

    private Race _race;

    private void OnEnable()
    {
        if (_race != null)
            SubscribeToRace(true);
    }

    private void OnDisable()
    {
        if (_race != null)
            SubscribeToRace(false);
    }

    public void ApplyTrace(Race trace)
    {
        _race = trace;
        SubscribeToRace(true);
        _leaderboard.Initialize(_race.CountPlayers);
        _circlesMaximum.text = _race.CountCircles.ToString();
    }
    
    private void OnTimeChanged(int minutes, int seconds)
    {
        _timerMinutes.text = string.Format("{0:00}", minutes);
        _timerSeconds.text = string.Format("{0:00}", seconds);
    }

    private void OnLeaderboardChanged(IEnumerable<Car> leaderboard)
    {
        _leaderboard.UpdateLeaderboard(leaderboard.GetEnumerator());
    }

    private void OnCirclesChanged(int circle)
    {
        _circlesCurrent.text = circle.ToString();
    }

    private void OnPlayerCountChanged(int count)
    {
        _leaderboard.Initialize(count);
    }

    private void SubscribeToRace(bool isSubscribed)
    {
        if (isSubscribed)
        {
            _race.LeaderboardChanged += OnLeaderboardChanged;
            _race.CirclesChanged += OnCirclesChanged;
            _race.TimeChanged += OnTimeChanged;
            _race.CountPlayerChanged += OnPlayerCountChanged;
        }
        else
        {
            _race.LeaderboardChanged -= OnLeaderboardChanged;
            _race.CirclesChanged -= OnCirclesChanged;
            _race.TimeChanged -= OnTimeChanged;
            _race.CountPlayerChanged -= OnPlayerCountChanged;
        }
    }
}
