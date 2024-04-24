using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class MapLoader : MonoBehaviourPunCallbacks
{
    private const int MaxPlayers = 4;

    [SerializeField] private string _levelName = "Game";
    [SerializeField] private RaceSelector _raceSelector;
    [SerializeField] private PlayerProperties _properties;
    [SerializeField] private Button _startGame;

    private void Awake()
    {
        _startGame.interactable = true;
        _startGame.onClick.AddListener(OnClickStartGame);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            roomProperties["Race"] = (int)_raceSelector.Current;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        }
        else if ((int)PhotonNetwork.CurrentRoom.CustomProperties["Race"] != (int)_raceSelector.Current)
        {
            PhotonNetwork.LeaveRoom();
            return;
        }

        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.LoadLevel(_levelName);
    }

    private void OnClickStartGame()
    {
        SetProperties();
        PhotonNetwork.JoinRandomRoom(new Hashtable() { { "Race", (int)_raceSelector.Current } }, 0);
        _startGame.interactable = false;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = MaxPlayers;
        roomOptions.CustomRoomProperties = new Hashtable() { { "Race", (int)_raceSelector.Current } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "Race" };
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public void SetProperties()
    {
        Hashtable Properties = PhotonNetwork.LocalPlayer.CustomProperties;
        Properties["ColorR"] = _properties.Values[TypesOfPlayerProperties.ColorR];
        Properties["ColorG"] = _properties.Values[TypesOfPlayerProperties.ColorG];
        Properties["ColorB"] = _properties.Values[TypesOfPlayerProperties.ColorB];
        PhotonNetwork.LocalPlayer.SetCustomProperties(Properties);
        PhotonNetwork.LocalPlayer.NickName = _properties.Name;
    }
}
