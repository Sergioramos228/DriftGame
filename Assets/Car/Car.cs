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

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        MyId = _photonView.ViewID;
        IsMine = _photonView.IsMine;
    }

    private void OnEnable()
    {
        _drift.ScoreChange += OnScoreChanged;
    }

    private void OnDisable()
    {
        _drift.ScoreChange -= OnScoreChanged;
    }

    private void OnExitTime()
    {
        Finish(0);
    }

    public void ConnectToRace(Race race)
    {
        if (_race != null)
            return;

        _race = race;
        _race.WeFinished += Finish;
        _race.HasExitTime += OnExitTime;
    }

    private void Finish(int place)
    {
        FinishDriftCoefficients finishDriftCoefficients = new FinishDriftCoefficients();
        float coefficient = finishDriftCoefficients.GetCoefficient(place);
        float gold = Drift * coefficient;
        ExitGames.Client.Photon.Hashtable mySettings = _photonView.Owner.CustomProperties;

        if (mySettings.ContainsKey("Gold"))
        {
            float currentGold = (float)mySettings["Gold"];
            mySettings["Gold"] = currentGold + gold;
            _photonView.Owner.CustomProperties = mySettings;
        }

        _controller.enabled = false;
        _drift.enabled = false;
        _drift.ScoreChange -= OnScoreChanged;
    }

    private void OnScoreChanged(float score)
    {
        Drift = score;
        ValueChanged?.Invoke();
    }
}
