using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ball : MonoBehaviourPun
{
    private Rigidbody2D rb;
    [SerializeField] private PhotonView pv;
    GameManager _manager;
    private int strokeCount;
    private string playerNick;

    public PhotonView Pv { get => pv; }
    public GameManager Manager { get => _manager; set => _manager = value; }
    public int StrokeCount { get => strokeCount; set => strokeCount = value; }

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _manager.UpdatePlayerName();
    }

    public void Move(Vector2 ballPos, Vector2 releasePos, float forceMultiplier)
    {
        rb.AddForce((ballPos - releasePos) * forceMultiplier);
        StrokeCounter();
    }

    public void StrokeCounter()
    {
        strokeCount++;
    }

    public void BallFinishedHole()
    {
        pv.RPC("DisappearBall", RpcTarget.All);
    }

    public void SetName(string nickName)
    {
        playerNick = nickName;
    }

    [PunRPC]
    public void DisappearBall()
    {
        gameObject.SetActive(false);
    }
}
