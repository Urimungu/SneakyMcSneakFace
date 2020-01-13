using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //References
    public static GameManager GM;

    [Header("References")]
    public GameObject Player;

    //Creates a Singleton for the Game Manager
    private void Awake() {
        if (GM == null) {
            GM = this;
            DontDestroyOnLoad(gameObject);
        }else {
            Destroy(gameObject);
        }

        //Sets the References
        Player = GameObject.FindGameObjectWithTag("Player");
    }
}
