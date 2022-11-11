using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public void SetManager(Ball ball)
    {
        ball.Manager = this;
    }
    private void Start()
    {
        
    }
}
