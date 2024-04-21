using System.Collections.Generic;
using UnityEngine;

public class RaceLeaderboard : MonoBehaviour
{
    [SerializeField] private List<RaceLeaderboardPlayerView> _players;

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

        for (int i = 0; i < count; i++)
            _players[i].ChangeVision(1);

        for (int i = _count; i < _players.Count; i++)
            _players[i].ChangeVision(0);
    }
}
