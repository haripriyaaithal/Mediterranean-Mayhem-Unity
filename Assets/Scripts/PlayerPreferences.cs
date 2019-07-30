using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPreferences : MonoBehaviour {

    [SerializeField] private Slider aimingSlider;

    // Start is called before the first frame update
    void Start() {
        aimingSlider.value = PlayerPrefs.GetFloat("AimingSensitivity", 20f);

    }

    public void UpdateAimingSensitivity(float senstivity) {
        PlayerPrefs.SetFloat("AimingSensitivity", senstivity);
    }
}
