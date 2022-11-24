using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.Linq;
public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private PhotonView pv;
    [SerializeField] private Button masterStartButton;
    [SerializeField] private GameObject waitingForMasterText, winText, loseText;

    private bool isGameStarted = false;

    public bool IsGameStarted { get => isGameStarted; set => isGameStarted = value; }

    private List<Ball> ballList;
    private void Start()
    {
        SetStartRequirements();
        ballList = new List<Ball>();
    }

    public void SetManager(Ball ball)
    {
        ball.Manager = this;
    }

    private void SetStartRequirements()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            masterStartButton.gameObject.SetActive(true);
        }
        else
        {
            waitingForMasterText.SetActive(true);
        }
    }

    // Activated by masterStartButton.
    public void StartGameButtonPressed()
    {
        pv.RPC("GameStarted", RpcTarget.All);
    }

    [PunRPC]
    public void GameStarted()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            masterStartButton.gameObject.SetActive(false);
        }
        else
        {
            waitingForMasterText.SetActive(false);
        }
        StartGame();
    }

    private void StartGame()
    {
        isGameStarted = true;
        ballList = GameObject.FindObjectsOfType<Ball>().ToList();
    }
}
