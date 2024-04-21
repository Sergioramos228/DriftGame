using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Completer : MonoBehaviour
{
    [SerializeField] private List<Button> _exitButtons;
    [SerializeField] private List<Button> _returnButtons;

    private void OnEnable()
    {
        foreach (Button exitButton in _exitButtons)
            exitButton.onClick.AddListener(OnExitButton);

        foreach (Button returnButton in _returnButtons)
            returnButton.onClick.AddListener(OnLoadLobbyScene);
    }

    private void OnDisable()
    {
        foreach (Button exitButton in _exitButtons)
            exitButton.onClick.RemoveListener(OnExitButton);

        foreach (Button returnButton in _returnButtons)
            returnButton.onClick.RemoveListener(OnLoadLobbyScene);
    }

    private void OnLoadLobbyScene()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }

    private void OnExitButton()
    {
        //Environment.Exit(0);
        Application.Quit();
    }
}
