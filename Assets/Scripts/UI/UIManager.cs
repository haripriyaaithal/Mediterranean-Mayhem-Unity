using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class UIManager : MonoBehaviour {

    [SerializeField] GameObject gamePlayUI;
    [SerializeField] GameObject gameOverUI;

    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI kills;
    [SerializeField] Image healthBar;

    [SerializeField] TextMeshProUGUI gameOverKills;
    [SerializeField] TextMeshProUGUI gameOverTime;

    private float currentTime = 0f;
    private StringBuilder timeText;

    // Start is called before the first frame update
    void Start() {
        timeText = new StringBuilder();
    }

    // Update is called once per frame
    void Update() {
        currentTime += Time.deltaTime;
        timer.text = GetTimeInFormat(currentTime).ToString();
    }


    private StringBuilder GetTimeInFormat(float time) {
        int seconds = (int)time % 60;

        int minutes = (int)time / 60;

        timeText.Clear();
        timeText.Append(minutes.ToString("00"));
        timeText.Append(':');
        timeText.Append(seconds.ToString("00"));

        return timeText;
    }

    public void UpdateKills(int numberOfKills) {
        kills.text = numberOfKills.ToString("00");
    }

    public void UpdatePlayerHealth(float health) {
        healthBar.fillAmount = health / 100;
    }


    public void ShowGameOverUI() {
        StartCoroutine(ShowGameOver());

    }

    private IEnumerator ShowGameOver() {
        yield return new WaitForSeconds(1.25f);
        gamePlayUI.SetActive(false);
        gameOverUI.SetActive(true);

        gameOverKills.text = kills.text;
        gameOverTime.text = timer.text;
    }
}
