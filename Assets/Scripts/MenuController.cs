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

}
