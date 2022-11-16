using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Hole : MonoBehaviourPun
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball)
        {
            ball.BallFinishedHole();
        }
    }
}
