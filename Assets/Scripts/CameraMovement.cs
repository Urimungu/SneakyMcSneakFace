using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //References
    private Transform Target;
    private Transform TF;

    public bool CanMove = true;
    public float Speed = 10;

    //Variables
    private bool attachPlayer = true;

    void Start() {
        //Finds the player from the Game Manager
        TF = GetComponent<Transform>();

        //If the player isn't set, then stop the game
        if (GameManager.Manager.Player == null)
            return;

        //Attaches the player if he exists
        FindPlayer();
        attachPlayer = false;
    }

    private void Update() {
        if (attachPlayer) {
            FindPlayer();
            return;
        }

        //Follows the player if able to move
        if(CanMove) {
            TF.position = Vector3.Slerp(transform.position, Target.position, Speed / 10);
        }
    }

    //Find the Player and attach him
    private void FindPlayer() {
        if(GameManager.Manager.Player == null)
            return;

        Target = GameManager.Manager.Player.transform;
        attachPlayer = false;
    }
}
