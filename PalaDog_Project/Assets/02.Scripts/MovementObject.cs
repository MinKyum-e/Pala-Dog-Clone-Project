using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementObject : MonoBehaviour
{
    [SerializeField]
    private VirtualJoystick virtualJoystick;
    private float moveSpeed = 10;
    [SerializeField]
    private Rigidbody2D rigid;
   
    private void Update()
    {
        float x = virtualJoystick.Horizontal();


        if (x > 0) x = 1;
        else if (x < 0) x = -1;
        rigid.velocity = new Vector2(x, 0) * moveSpeed;
    }

    //setter
    public void setMoveSpeed(float s)
    {
        moveSpeed = s;
    }
}