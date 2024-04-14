using System;
using UnityEngine;
using Photon.Pun;

public class Car : MonoBehaviour
{
    [SerializeField] private Transform _body;
    [SerializeField] private DriftProcessor _drift;
    [SerializeField] private NameView _nameView;

    public float Drift { get; private set; }
    public int MyId { get; private set; }
    public bool IsMine { get; private set; }
    public string Name => _nameView.Name;
    public Vector3 Body => _body.position;

    public event Action ValueChanged;

    private void Awake()
    {
        PhotonView view = GetComponent<PhotonView>();
        MyId = view.ViewID;
        IsMine = view.IsMine;
    }

    private void OnEnable()
    {
        _drift.ScoreChange += OnScoreChanged;
    }

    private void OnDisable()
    {
        _drift.ScoreChange -= OnScoreChanged;
    }

    private void OnScoreChanged(float score)
    {
        Drift = score;
        ValueChanged?.Invoke();
    }
}
