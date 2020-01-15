using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class EnemyController : MonoBehaviour
{
    //References
    public Sprite Walking, Chasing;
    private Transform TF;

    private bool IsChasing = false;

    private void Awake()
    {
        TF = transform.GetComponent<Transform>();
        ChangeState();
    }

    private void Update()
    {


    }

    //This is updated when he detects a player or doesn't detect them anymore.
    private void ChangeState() {
        TF.GetChild(0).GetComponent<SpriteRenderer>().sprite = IsChasing ? Chasing : Walking; ;

    }
}
