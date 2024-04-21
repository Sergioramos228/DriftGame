using System;
using UnityEngine;
using Photon.Pun;

public class Car : MonoBehaviour
{
    [SerializeField] private Transform _body;
    [SerializeField] private DriftProcessor _drift;
    [SerializeField] private NameView _nameView;
    [SerializeField] private CarController _controller;

    private Race _race;
    private PhotonView _photonView;

    public float Drift { get; private set; }
    public int MyId { get; private set; }
    public bool IsMine { get; private set; }
    public string Name => _nameView.Name;
    public Vector3 Body => _body.position;

    public event Action ValueChanged;
    public event Action<float> Finished;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        MyId = _photonView.ViewID;
        IsMine = _photonView.IsMine;

        if (IsMine == false)
            _drift.MoveToNetworkMode();
    }

    private void OnEnable()
    {
        _drift.ScoreChange += OnScoreChanged;
    }

    private void OnDisable()
    {
        _drift.ScoreChange -= OnScoreChanged;

        if (_race != null)
            _race.WeFinished -= Finish;
    }

    private void OnExitTime()
    {
        Finish(0);
    }

    public void ConnectToRace(Race race, RaceTimer timer)
    {
        if (_race != null)
            return;

        _race = race;
        _race.WeFinished += Finish;
        timer.HasExitTime += OnExitTime;
    }

    private void Finish(int place)
    {
        FinishDriftCoefficients finishDriftCoefficients = new FinishDriftCoefficients();
        float coefficient = finishDriftCoefficients.GetCoefficient(place);
        float gold = Drift * coefficient;
        Finished?.Invoke(gold);
        _controller.enabled = false;
        _drift.enabled = false;
        _drift.ScoreChange -= OnScoreChanged;
    }

    private void OnScoreChanged(float score)
    {
        Drift = score;
        ValueChanged?.Invoke();
        _photonView.RPC("OnScoreChangedRcp", RpcTarget.AllBuffered, score);
    }

    [PunRPC]
    private void OnScoreChangedRcp(float score)
    {
        Drift = score;
        ValueChanged?.Invoke();
    }
}
