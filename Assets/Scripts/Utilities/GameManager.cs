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
    [SerializeField] private TextMeshProUGUI timeText;

    private bool isGameStarted = false;
    private bool isGameFinished = false;
    private float currentTime = 0f;

    public bool IsGameStarted { get => isGameStarted; set => isGameStarted = value; }

    private List<Ball> ballList;
    public List<Ball> BallList { get => ballList; set => ballList = value; }
    private List<Player> winners;
    private List<Player> losers;
    public bool IsGameFinished { get => isGameFinished; set => isGameFinished = value; }
    public List<Player> Winners { get => winners; set => winners = value; }
    public List<Player> Losers { get => losers; set => losers = value; }

    private void Start()
    {
        SetStartRequirements();
        ballList = new List<Ball>();
        winners = new List<Player>();
        losers = new List<Player>();
    }

    private void Update()
    {
        SetTime();
    }

    public void GameFinished()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            List<Ball> ballWinners = new List<Ball>();
            List<Ball> ballLosers = new List<Ball>();

            ballWinners = CheckWinners(ballList);
            ballLosers = CheckLosers(ballList, ballWinners);

            MasterManager._instance.RPCMaster("RequestWinners", ballWinners);

            if(ballLosers != null)
            {
                MasterManager._instance.RPCMaster("RequestLosers", ballLosers);
            }

            foreach (var client in winners)
            {
                pv.RPC("ShowWinScreen", RpcTarget.All, client);
            }
            foreach (var client in losers)
            {
                pv.RPC("ShowLoseScreen", RpcTarget.All, client);
            }
        }
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

    public bool CheckAllBallsFinished()
    {
        int currentBalls = 0;
        foreach (var ball in ballList)
        {
            if (!ball.HasFinishedHole)
            {
                currentBalls++;
            }
        }

        if (currentBalls == 0)
        {
            return true;
        }
        else
            return false;
    }

    private List<Ball> CheckWinners(List<Ball> balls)
    {
        List<Ball> winners = new List<Ball>();
        int minStrokeCount = balls[0].StrokeCount;

        for (int i = 0; i < balls.Count; i++)
        {
            if(minStrokeCount >= balls[i].StrokeCount)
            {
                minStrokeCount = balls[i].StrokeCount;
            }
        }

        foreach (var ball in balls)
        {
            if(ball.StrokeCount == minStrokeCount)
            {
                winners.Add(ball);
            }
        }

        return winners;
    }

    private List<Ball> CheckLosers(List<Ball> balls, List<Ball> ballWinners)
    {
        List<Ball> losers = new List<Ball>();
        foreach (var ball in balls)
        {
            if (ballWinners.Contains(ball))
            {

            }
            else
            {
                losers.Add(ball);
            }
        }
        return losers;
    }

    [PunRPC]
    public void ShowWinScreen(Player player)
    {
        if (player == PhotonNetwork.LocalPlayer)
        {
            winText.SetActive(true);
        }
    }

    [PunRPC]
    public void ShowLoseScreen(Player client)
    {
        if (client == PhotonNetwork.LocalPlayer)
        {
            loseText.SetActive(true);
        }
    }

    private int minutes, seconds;
    private void SetTime()
    {
        if (!IsGameStarted) return;
        if (PhotonNetwork.IsMasterClient == false) return;

        currentTime += Time.deltaTime;
        minutes = (int)(currentTime / 60f);
        seconds = (int)(currentTime - minutes * 60f);

        pv.RPC("UpdateTime", RpcTarget.All, minutes, seconds);
    }

    [PunRPC]
    public void UpdateTime(int minutes, int seconds)
    {
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
