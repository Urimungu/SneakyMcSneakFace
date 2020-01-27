using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Random = System.Random;

public class EnemyController : MonoBehaviour
{
    //References
    public Sprite Walking, Chasing;
    private Transform TF;
    private Rigidbody2D rb2d;

    //Stats
    public float Speed = 10;
    public float SpeedCap = 10;
    public float runSpeedCap = 10;
    public float stopTime = 3;
    public float RotationSpeed = 10;


    //States
    public enum States { Patrol, Chase, Searching };
    public Transform[] searchPoint;


    private States state;
    private bool isRunning = false;
    private bool isChasing = false;
    private bool inRange = false;
    public Transform target;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        TF = transform;
        state = States.Patrol;
        target = searchPoint[0];
    }

    private void Update() {
        ChangeState();
    }

    //This is the finite state-machine that the enemy will use
    private void ChangeState() {
        switch (state) {
            case States.Patrol:
                Patrol();
                Movement();
            break;
            case States.Searching:
                break;
            case States.Chase:
                break;
        }
        //TF.GetChild(0).GetComponent<SpriteRenderer>().sprite = isChasing ? Chasing : Walking;
    }

    private void Patrol() {
        if (inRange) {
            print("Test");
            //Chooses a new target from the list and makes sure it's not the one that was already selected
            int randomLocation = UnityEngine.Random.Range(0, searchPoint.Length);
            if (target == searchPoint[randomLocation] && searchPoint.Length > 0)
            {
                if (randomLocation == 0)
                    randomLocation = 1;
                else if (randomLocation == searchPoint.Length - 1)
                    randomLocation = 0;
                else
                    ++randomLocation;
            }

            //Sets the location
            target = searchPoint[randomLocation];
        }


    }

    private void Movement() {
        //Look at target and moves towards it
        Vector2 newPos = target.position - TF.position;

        Vector3 newRot = Vector3.RotateTowards(TF.forward, newPos, RotationSpeed, 10);
        TF.rotation = Quaternion.LookRotation(newRot);

        //rb2d.AddForce(TF.forward * Speed);

        //Provides a counter speed to slow the enemy down
        if (Mathf.Abs(rb2d.velocity.magnitude) > (isChasing ? runSpeedCap : SpeedCap))
            rb2d.AddForce(-rb2d.velocity.normalized * (isChasing ? runSpeedCap : SpeedCap));

        //Slows down if it gets in range with its target
        if (Mathf.Abs((target.position - TF.position).magnitude) < 0.1f && Mathf.Abs(rb2d.velocity.magnitude) > 0.1f) {
            rb2d.AddForce(-rb2d.velocity.normalized * stopTime);
        }
    }
}
