using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variables
    public float SpeedAcc = 10;
    public float SpeedCap = 15;
    public float StopSpeed = 10;
    public float RotateSpeed = 10;
    public bool CanMove = true;

    //References
    private Rigidbody2D rb2d;
    private Transform TF;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        TF = GetComponent<Transform>();
    }

    void Update()
    {
        if (CanMove)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Movement(horizontal, vertical);
        }
    }

    private void Movement(float hor, float ver){
        //Removes angle speed boost
        Vector2 newDir = new Vector2(hor, ver).normalized;

        //Moves the player Up/Down/Left/Right
        rb2d.AddForce(newDir * SpeedAcc);


        //Stops the player after he has reached a certain speed
        if(Mathf.Abs(rb2d.velocity.magnitude) > SpeedCap)
            rb2d.AddForce(-rb2d.velocity.normalized * SpeedAcc);

        //Slows the player down after they have stopped moving and they are no longer pressing anything
        if (Mathf.Abs(rb2d.velocity.magnitude) > 0.2f) {
            TF.rotation = Quaternion.LookRotation(transform.forward, rb2d.velocity.normalized);
            if (Mathf.Abs(newDir.magnitude) < 0.1f)
                rb2d.AddForce(-rb2d.velocity.normalized * StopSpeed);
        }
    }
}
