using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Image HealthBar;

    public Image DetectionStat;
    public Sprite Hidden, Heard, Spotted;

    private void Update() {
        ChangeHiddenState();

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

}
