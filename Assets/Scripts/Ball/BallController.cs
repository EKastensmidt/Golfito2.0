using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : Ball
{
    bool didMouseUp;
    bool didMouseDown;

    Vector2 ballPos;
    Vector2 releasePos;
    public override void Start()
    {
        base.Start();
        MouseReset();
    }

    private void OnMouseDown()
    {
        ballPos = transform.position;
        didMouseDown = true;
    }

    private void OnMouseUp()
    {
        releasePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        didMouseUp = true;
    }

    public override void Update()
    {
        if(didMouseUp && didMouseDown)
        {
            rb.AddForce((ballPos - releasePos) * forceMultiplier);
            MouseReset();
        }
    }

    private void MouseReset()
    {
        didMouseUp = false;
        didMouseDown = true;
    }
}
