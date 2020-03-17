using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variables
    public float Speed = 2;
    public bool CanMove = true;

    //References
    private Rigidbody2D rb2d;
    private Transform TF;

    void Start() {
        //Sets references
        rb2d = GetComponent<Rigidbody2D>();
        TF = GetComponent<Transform>();

        //Makes sure that a Game Manager Exists
        if (GameManager.Manager == null)
            return;

        GameManager.Manager.Player = gameObject;
    }

    void Update() {
        if (CanMove) {
            //Gets inputs and calls the movement function
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Movement(horizontal, vertical);
        }
    }

    private void Movement(float hor, float ver){
        //Removes angle speed boost
        Vector2 newDir = new Vector2(hor, ver).normalized;

        //Moves the player Up/Down/Left/Right
        rb2d.velocity = newDir * Speed;

        //Handles Rotation
        if (rb2d.velocity.magnitude > 0.1f)
            TF.rotation = Quaternion.LookRotation(transform.forward, rb2d.velocity.normalized);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Money")) {
            GameManager.Manager.CurrentCoins++;
            Destroy(other.gameObject);
        }
    }
}
