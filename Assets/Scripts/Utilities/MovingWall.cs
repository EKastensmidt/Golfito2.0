using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MovingWall : MonoBehaviourPun
{
    [SerializeField] float changeDir  = 2f;
    [SerializeField] float speed = 0.25f;
    float cd = 0f;
    bool goingRight = true;
    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(cd <= 0)
            {
                if (goingRight)
                    goingRight = false;
                else
                    goingRight = true;

                cd = changeDir;
            }

            if (goingRight)
            {
                transform.Translate(Vector2.up * Time.deltaTime * speed);

            }
            else
            {
                transform.Translate(Vector2.down * Time.deltaTime * speed);
            }

            cd -= Time.deltaTime;
        }
    }
}
