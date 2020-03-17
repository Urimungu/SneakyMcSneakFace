using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    //Scene Loading
    public void ChangeScene(string state) {
        GameManager.Manager.SceneManagerState(state);
    }
}
