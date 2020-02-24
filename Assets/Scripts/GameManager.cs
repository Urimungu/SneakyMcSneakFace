using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class GameManager : MonoBehaviour
{
    //References
    public static GameManager Manager;

    [Header("References")]
    public GameObject Player;


    //Enum
    public enum DetectionLevel { Hidden, Heard, Spotted}
    public DetectionLevel DetectedLevel;
    public int Heard, Spotters;

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

    private void Update() {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("LevelOne"))
            RunDetectionLevel();
    }

    private void RunDetectionLevel() {
        if (Spotters >= 1)
            DetectedLevel = DetectionLevel.Spotted;
        else if (Heard >= 1)
            DetectedLevel = DetectionLevel.Heard;
        else
            DetectedLevel = DetectionLevel.Hidden;

    }

}
