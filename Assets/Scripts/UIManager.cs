using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Image HealthBar;

    public Image DetectionStat;
    public Text Message, CoinCount;
    public Sprite Hidden, Heard, Spotted;
    public Image Lose;

    private bool _inRange;

    private void Awake() {
        GameManager.Manager.UImanager = this;
        Message.text = "";
    }

    private void Update() {
        ChangeHiddenState();
        string color = GameManager.Manager.CurrentCoins == GameManager.Manager.TotalCoins ? "<Color=#00ff00>" : "<Color=#ff0000>";
        CoinCount.text = color + GameManager.Manager.CurrentCoins + "</color> /" + GameManager.Manager.TotalCoins + " Coins";
    }

    public void SendMessage(string message) {
        if(!_inRange) {
            _inRange = true;
            Message.text = message;
            StartCoroutine(DetectionMessage());
        }
    }

    IEnumerator DetectionMessage() {
        yield return new WaitForSeconds(5);
        Message.text = "";
        _inRange = false;
    }

    public void ChangeHiddenState() {
        switch (GameManager.Manager.DetectedLevel) {
            case GameManager.DetectionLevel.Hidden:
                DetectionStat.sprite = Hidden;
                break;
            case GameManager.DetectionLevel.Heard:
                DetectionStat.sprite = Heard;
            break;
            case GameManager.DetectionLevel.Spotted:
                DetectionStat.sprite = Spotted;
            break;
        }
    }

    public void SetLoseAlpha(float numb) {
        var tempColor = Lose.color;
        tempColor.a += numb;
        tempColor.a = Mathf.Clamp(tempColor.a, 0, 0.5f);
        Lose.color = tempColor;
    }

}
