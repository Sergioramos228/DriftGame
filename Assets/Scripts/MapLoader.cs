using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class MapLoader : MonoBehaviourPunCallbacks
{
    [SerializeField] private string _levelName = "Game";
    [SerializeField] private TMP_InputField _temp;
    [SerializeField] private TraceSelector _raceSelector;
    [SerializeField] private ColorPicker _colorPicker;
    [SerializeField] private Button _startGame;

    private void Awake()
    {
        PUNSerializationService.Register();
        _startGame.onClick.AddListener(OnClickStartGame);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;

        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            roomProperties["Trace"] = (int)_raceSelector.Current;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        }

        PhotonNetwork.LoadLevel(_levelName);
    }

    private void OnClickStartGame()
    {
        SetProperties();
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public void SetProperties()
    {
        Hashtable Properties = PhotonNetwork.LocalPlayer.CustomProperties;
        Properties.Add("Settings", new CarBuild(_colorPicker.Color));
        PhotonNetwork.LocalPlayer.SetCustomProperties(Properties);
        PhotonNetwork.LocalPlayer.NickName = _temp.text;
    }
}
