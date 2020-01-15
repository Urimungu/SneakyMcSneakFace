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

    void Start() {
        //Finds the player from the Game Manager
        Target = GameManager.GM.Player.transform;
        TF = GetComponent<Transform>();
    }

    private void Update()
    {
        //Follows the player if able to move
        if (CanMove) {
            TF.position = Vector3.Slerp(transform.position, Target.position, Speed / 10);
        }
    }

}
