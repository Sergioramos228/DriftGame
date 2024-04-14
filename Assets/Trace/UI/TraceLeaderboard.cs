using System.Collections.Generic;
using UnityEngine;

public class TraceLeaderboard : MonoBehaviour
{
    [SerializeField] private List<TraceLeaderboardPlayerView> _players;

    private int _count;

    private void Awake()
    {
        _count = _players.Count;
    }

    public void UpdateLeaderboard(IEnumerator<Car> players)
    {
        for (int i = 0; i < _count && players.MoveNext(); i++)
        {
            _players[i].Show(players.Current);
        }
    }

    public void Initialize(int count)
    {
        _count = count;

        for (int i = _count; i < _players.Count; i++)
        {
            _players[i].Hide();
        }
    }
}
