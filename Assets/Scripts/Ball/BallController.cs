using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallController : MonoBehaviourPun
{
    bool didMouseUp;
    bool didMouseDown;

    Vector2 ballPos;
    Vector2 releasePos;
    [SerializeField] protected float forceMultiplier;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Destroy(Camera.main.gameObject);
            Destroy(this);
        }
    }

    public void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            MasterManager._instance.RPCMaster("RequestConnectPlayer", PhotonNetwork.LocalPlayer);
        }

        MouseReset();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ballPos = transform.position;
            didMouseDown = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            releasePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            didMouseUp = true;
        }

        if(didMouseUp && didMouseDown)
        {
            MasterManager._instance.RPCMaster("RequestMoveBall", PhotonNetwork.LocalPlayer, ballPos, releasePos, forceMultiplier);
            MouseReset();
        }
    }

    private void MouseReset()
    {
        didMouseUp = false;
        didMouseDown = true;
    }
}
