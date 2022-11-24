using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Ball : MonoBehaviourPun
{
    private Rigidbody2D rb;
    [SerializeField] private TextMeshPro playerName;
    [SerializeField] private TextMeshPro score;
    [SerializeField] private PhotonView pv;
    GameManager _manager;
    private int strokeCount;
    private string playerNick =  null;
    private bool hasFinishedHole = false;

    public PhotonView Pv { get => pv; }
    public GameManager Manager { get => _manager; set => _manager = value; }
    public int StrokeCount { get => strokeCount; set => strokeCount = value; }
    public string PlayerNick { get => playerNick; set => playerNick = value; }
    public bool HasFinishedHole { get => hasFinishedHole; set => hasFinishedHole = value; }

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!_manager.IsGameStarted)
            return;

        if (playerNick == null)
        {
            MasterManager._instance.RPCMaster("RequestPlayerName", this);
            pv.RPC("UpdateName", RpcTarget.All, playerNick);
        }
    }

    public void Move(Vector2 ballPos, Vector2 releasePos, float forceMultiplier)
    {
        if (!_manager.IsGameStarted)
            return;

        rb.AddForce((ballPos - releasePos) * forceMultiplier);
        StrokeCounter();
    }

    public void StrokeCounter()
    {
        strokeCount++;
        pv.RPC("UpdateScore", RpcTarget.AllBuffered, strokeCount.ToString());
    }

    public void BallFinishedHole()
    {
        pv.RPC("DisappearBall", RpcTarget.All);
    }

    [PunRPC]
    public void DisappearBall()
    {
        hasFinishedHole = true;

        if (PhotonNetwork.IsMasterClient)
        {
            if (_manager.CheckAllBallsFinished())
            {
                _manager.GameFinished();
            }
        }

        gameObject.SetActive(false);
    }

    [PunRPC]
    public void UpdateScore(string strokes)
    {
        score.text = strokes;
    }

    [PunRPC]
    public void UpdateName(string player)
    {
        playerName.text = player;
    }
}
