using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //References
    public static GameManager Manager;

    [Header("References")]
    public GameObject Player;

    //Creates a Singleton for the Game Manager
    private void Awake() {
        #region Singleton
        if (Manager == null) {
            Manager = this;
            DontDestroyOnLoad(gameObject);
        }else {
            Destroy(gameObject);
        }
        #endregion
    }
}
