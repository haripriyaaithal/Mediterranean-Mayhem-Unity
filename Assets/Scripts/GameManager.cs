using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] GameObject mobileSpecificUI;


    private void Awake() {
        Application.targetFrameRate = 60;

        // Disable joystick UI for non Android devices
        if (Application.platform != RuntimePlatform.Android) {
            mobileSpecificUI.SetActive(false);
        }
    }
}