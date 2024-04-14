using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TraceView : MonoBehaviour
{
    [SerializeField] private TraceLeaderboard _leaderboard;
    [SerializeField] private TMP_Text _timerMinutes;
    [SerializeField] private TMP_Text _timerSeconds;
    [SerializeField] private TMP_Text _circlesCurrent;
    [SerializeField] private TMP_Text _circlesMaximum;

    private Trace _trace;

    private void OnEnable()
    {
        if (_trace != null)
            SubscribeToTrace(true);
    }

    private void OnDisable()
    {
        if (_trace != null)
            SubscribeToTrace(false);
    }

    public void ApplyTrace(Trace trace)
    {
        _trace = trace;
        SubscribeToTrace(true);
        _leaderboard.Initialize(_trace.CountPlayers);
        _circlesMaximum.text = _trace.CountCircles.ToString();
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

    private void SubscribeToTrace(bool isSubscribed)
    {
        if (isSubscribed)
        {
            _trace.LeaderboardChanged += OnLeaderboardChanged;
            _trace.CirclesChanged += OnCirclesChanged;
            _trace.TimeChanged += OnTimeChanged;
            _trace.CountPlayerChanged += OnPlayerCountChanged;
        }
        else
        {
            _trace.LeaderboardChanged -= OnLeaderboardChanged;
            _trace.CirclesChanged -= OnCirclesChanged;
            _trace.TimeChanged -= OnTimeChanged;
            _trace.CountPlayerChanged -= OnPlayerCountChanged;
        }
    }
}
