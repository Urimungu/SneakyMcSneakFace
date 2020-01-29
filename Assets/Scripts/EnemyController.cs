﻿using UnityEngine;
using System.Collections.Generic;

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
    public List<Transform> searchPoint = new List<Transform>();

    //Misc
    private States state;
    private bool isRunning = false;
    private bool isChasing = false;
    private bool inRange = false;
    public Transform target;
    private float timer;

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
                break;
            case States.Searching:
                break;
            case States.Chase:
                break;
        }
        //TF.GetChild(0).GetComponent<SpriteRenderer>().sprite = isChasing ? Chasing : Walking;
    }

    private void Patrol() {
        if(inRange) {
            //Chooses a new target from the list and makes sure it's not the one that was already selected
            int randomLocation = Random.Range(0, searchPoint.Count);
            if(target == searchPoint[randomLocation] && searchPoint.Count > 0) {
                if(randomLocation == 0)
                    randomLocation = 1;
                else if(randomLocation == searchPoint.Count - 1)
                    randomLocation = 0;
                else
                    ++randomLocation;
            }

            //Sets the location
            target = searchPoint[randomLocation];
            inRange = false;
        }
        if(timer < Time.time)
            Movement();
    }

    private void Movement() {
        //Look at target and moves towards it
        Vector2 newPos = target.position - TF.position;

        float angle = Mathf.Atan2(newPos.y, newPos.x) * Mathf.Rad2Deg;
        TF.rotation = Quaternion.AngleAxis(Mathf.LerpAngle(TF.eulerAngles.magnitude, angle, 0.1f), Vector3.forward);

        rb2d.AddForce(TF.right * Speed);

        //Provides a counter speed to slow the enemy down
        if (Mathf.Abs(rb2d.velocity.magnitude) > (isRunning ? runSpeedCap : SpeedCap)) {
            rb2d.AddForce(-rb2d.velocity.normalized * (isRunning ? runSpeedCap : SpeedCap));
        }

        //Slows down if it gets in range with its target
        if (Mathf.Abs((target.position - TF.position).magnitude) < 0.5f && Mathf.Abs(rb2d.velocity.magnitude) > 0.1f) {
            rb2d.velocity = Vector3.zero;
            rb2d.AddForce(-rb2d.velocity.normalized * stopTime);
            timer = Time.time + Random.Range(2f, 6f);
            inRange = true;
        }
    }
}
