using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MasterManager : MonoBehaviourPunCallbacks
{
    public GameManager gameManager;
    public static MasterManager _instance;

    Dictionary<Player, Ball> _dicChars = new Dictionary<Player, Ball>();
    Dictionary<Ball, Player> _dicPlayer = new Dictionary<Ball, Player>();

    public void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    // RPC CALL
    public void RPCMaster(string name, params object[] p)
    {
        RPC(name, PhotonNetwork.MasterClient, p);
    }
    public void RPC(string name, Player target, params object[] p)
    {
        photonView.RPC(name, target, p);
    }

    //RPCs
    [PunRPC]
    public void RequestMoveBall(Player client, Vector2 ballPos, Vector2 releasePos, float forceMultiplier)
    {
        if (_dicChars.ContainsKey(client))
        {
            Ball ball = _dicChars[client];
            ball.Move(ballPos, releasePos, forceMultiplier);
        }
    }

    [PunRPC]
    public void RequestConnectPlayer(Player client)
    {
        GameObject obj = PhotonNetwork.Instantiate("Ball", transform.position, Quaternion.identity);
        Ball ball = obj.GetComponent<Ball>();
        photonView.RPC("UpdatePlayer", RpcTarget.All, client, ball.Pv.ViewID);
    }

    [PunRPC]
    public void UpdatePlayer(Player client, int id)
    {
        PhotonView pv = PhotonView.Find(id);
        Ball ball = pv.gameObject.GetComponent<Ball>();
        _dicChars[client] = ball;
        _dicPlayer[ball] = client;
        gameManager.SetManager(ball);
    }

    [PunRPC]
    public void RequestPlayerName(Ball ball)
    {
        if (_dicPlayer.ContainsKey(ball))
        {
            Player player = _dicPlayer[ball];
            ball.PlayerNick = player.NickName;
        }
    }

    [PunRPC]
    public void RequestWinners(List<Ball> balls)
    {
        foreach (var ball in balls)
        {
            if (_dicPlayer.ContainsKey(ball))
            {
                Player player = _dicPlayer[ball];
                gameManager.Winners.Add(player);
            }
        }
    }

    [PunRPC]
    public void RequestLosers(List<Ball> balls)
    {
        foreach (var ball in balls)
        {
            if (_dicPlayer.ContainsKey(ball))
            {
                Player player = _dicPlayer[ball];
                gameManager.Losers.Add(player);
            }
        }
    }
}
