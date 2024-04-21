using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class StartRacing : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _startRace;
    [SerializeField] private RaceLoader _loader;

    private PhotonView _photonView;

    public event Action TraceHasStarted;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _startRace.onClick.AddListener(OnStartRaceButtonClick);

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        _startRace.onClick.RemoveListener(OnStartRaceButtonClick);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (PhotonNetwork.IsMasterClient)
            _startRace.gameObject.SetActive(true);
    }

    private void OnStartRaceButtonClick()
    {
        _photonView.RPC("LoadScene", RpcTarget.OthersBuffered);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        _loader.Load();
        _startRace.gameObject.SetActive(false);
    }

    [PunRPC]
    private void LoadScene()
    {
        _loader.Load();
    }
}
