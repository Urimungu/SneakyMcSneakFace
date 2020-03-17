using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public GameObject CoinHolder;

    private void Awake() {
        GameManager.Manager.TotalCoins = CoinHolder.transform.childCount;
    }

    void Update() {
        if(Vector3.Distance(GameManager.Manager.Player.transform.position, transform.position) < 2) {
            if(GameManager.Manager.CurrentCoins != GameManager.Manager.TotalCoins) {
                GameManager.Manager.UImanager.SendMessage("You cannot leave without all of the cash.");
            } else {
                GameManager.Manager.SceneManagerState("WinScreen");
            }
        }
    }
}
