using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class GameManager : MonoBehaviour
{
    //References
    public static GameManager Manager;
    public UIManager UImanager;

    [Header("References")]
    public GameObject Player;


    //Enum
    public enum DetectionLevel { Hidden, Heard, Spotted}
    public DetectionLevel DetectedLevel;
    public int Heard, Spotters;
    public int TotalCoins, CurrentCoins;

    //Variables
    private bool _isRunning;

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
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("LevelOne"))
            return;
        
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

    //Scene Loader Finite State Machine
    public void SceneManagerState(string state) {
        switch(state) {
            case "TitleScreen": SceneManager.LoadScene("TitleScreen");  break;
            case "LevelOne": 
                SceneManager.LoadScene("LevelOne");
                CurrentCoins = 0;
            break;
            case "WinScreen": SceneManager.LoadScene("WinScreen");    break;
            case "LoseScreen": SceneManager.LoadScene("LoseScreen");   break;
            case "Quit": Application.Quit(); break;
            default:    print("Invalid State Detected.");  break;
        }
    }

    public void CaughtPlayer() {
        Player.GetComponent<PlayerMovement>().CanMove = false;
        Player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        UImanager.SendMessage("YOU HAVE BEEN CAUGHT");
        if(!_isRunning) {
            _isRunning = true;
            StartCoroutine(LoseScreen());
        }
    }

    IEnumerator LoseScreen() {
        for(int i = 0; i < 60; i++) {
            yield return new WaitForSeconds(0.1f);
            Camera.main.orthographicSize -= 0.03f;
            UImanager.SetLoseAlpha(0.03f);
        }

        SceneManagerState("LoseScreen");
        _isRunning = false;
    }
}
