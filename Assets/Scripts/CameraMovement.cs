using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //References
    private Transform Target;

    public bool CanMove = true;
    public float Speed = 10;

    void Start() {
        //Finds the player from the Game Manager
        Target = GameManager.GM.Player.transform;
    }

    private void Update()
    {
        //Follows the player if able to move
        if (CanMove) {
            transform.position = Vector3.Slerp(transform.position, Target.position, Speed / 10);
        }
    }

}
