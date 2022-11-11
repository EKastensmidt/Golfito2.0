using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ball : MonoBehaviourPun
{
    protected Rigidbody2D rb;
    [SerializeField] private PhotonView pv;

    public PhotonView Pv { get => pv; }

    GameManager _manager;
    public GameManager Manager { set => _manager = value; }

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Update()
    {

    }

    public void Move(Vector2 ballPos, Vector2 releasePos, float forceMultiplier)
    {
        rb.AddForce((ballPos - releasePos) * forceMultiplier);
    }
}
