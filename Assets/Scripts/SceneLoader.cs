using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    //Scene Loading
    public void MainMenu() { SceneManager.LoadScene("TitleScreen"); }
    public void StartGame() { SceneManager.LoadScene("LevelOne"); }
    public void WinGame() { SceneManager.LoadScene("WinScreen"); }
    public void LoseGame() { SceneManager.LoadScene("LoseScreen"); }
    public void QuitGame() { Application.Quit(); }
}
