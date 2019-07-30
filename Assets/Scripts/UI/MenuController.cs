using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public void Quit() {
        Application.Quit();
    }

    public void LoadGame() {
        SceneManager.LoadScene("GamePlay");
    }

    public void MainMenuExitAnimation() {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("mainMenuExit", true);
    }

    public void GoBack() {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (buildIndex > 0) {
            SceneManager.LoadScene(buildIndex - 1);
        }
    }

    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
