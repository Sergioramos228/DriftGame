using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class RaceTimer : MonoBehaviourPunCallbacks
{
    private const int SecondsInMinute = 60;
    private const int Second = 1;

    [SerializeField] private int _raceSeconds = 120;

    private int _minutes;
    private int _seconds;
    private PhotonView _photonView;
    private Coroutine _current;

    public event Action<int, int> TimeChanged;
    public event Action HasExitTime;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Countdown();
    }

    public void Countdown()
    {
        if (PhotonNetwork.IsMasterClient && _current == null)
            _current = StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        WaitForSeconds second = new WaitForSeconds(Second);
        yield return null;

        while (_raceSeconds > 0)
        {
            _raceSeconds -= Second;
            _seconds = _raceSeconds % SecondsInMinute;
            _minutes = (int)Math.Floor((decimal)_raceSeconds / SecondsInMinute);
            TimeChanged?.Invoke(_minutes, _seconds);
            _photonView.RPC("RcpTimeChange", RpcTarget.AllBuffered, _minutes, _seconds);
            yield return second;
        }

        _photonView.RPC("RcpHasExitTime", RpcTarget.AllBuffered);
        HasExitTime?.Invoke();
    }

    [PunRPC]
    private void RcpTimeChange(int minutes, int seconds)
    {
        _minutes = minutes;
        _seconds = seconds;
        TimeChanged?.Invoke(minutes, seconds);
    }

    [PunRPC]
    private void RcpHasExitTime()
    {
        HasExitTime?.Invoke();
    }
}
