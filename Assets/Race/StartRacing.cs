using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

[RequireComponent(typeof(PhotonView))]
public class StartRacing : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _startRace;
    [SerializeField] private RaceLoader _loader;
    [SerializeField] private TMP_Text _label;

    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        UpdateLabel();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
        _startRace.onClick.AddListener(OnStartRaceButtonClick);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
        _startRace.onClick.RemoveListener(OnStartRaceButtonClick);
    }

    public override void OnEnteredRoom(Player newPlayer)
    {
        base.OnEnteredRoom(newPlayer);
        UpdateLabel();
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        base.OnPlayerLeftRoom(player);
        UpdateLabel();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (PhotonNetwork.IsMasterClient)
            _startRace.gameObject.SetActive(true);
    }

    private void UpdateLabel()
    {
        _label.text = "Игроки: ";

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
            _label.text = $"{_label.text}\n{player.Key} - {player.Value.NickName}";

        _startRace.interactable = PhotonNetwork.CurrentRoom.Players.Count > 1 && PhotonNetwork.IsMasterClient;
    }

    private void OnStartRaceButtonClick()
    {
        _photonView.RPC("LoadScene", RpcTarget.OthersBuffered);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        _loader.Load();
    }

    [PunRPC]
    private void LoadScene()
    {
        _loader.Load();
    }
}
