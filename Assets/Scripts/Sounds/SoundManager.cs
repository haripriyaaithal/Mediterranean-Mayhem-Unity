using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    [Header("Player")]
    [SerializeField] private AudioClip playerFire;
    [SerializeField] private AudioClip playerHit;
    [SerializeField] private AudioClip playerDie;

    [Header("Enemy")]
    [SerializeField] private AudioClip enemyAttack;
    [SerializeField] private AudioClip enemyDie;
    [SerializeField] private AudioClip enemyHit;


    private AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    #region Player Sounds

    public void PlayerFire() {
        audioSource.PlayOneShot(playerFire);
    }

    #endregion

    #region Enemy Sounds
    public void PlayEnemyAttack() {
        audioSource.PlayOneShot(enemyAttack);
    }

    public void PlayEnemyHit() {
        audioSource.PlayOneShot(enemyHit);
    }

    public void PlayEnemyDeath() {
        audioSource.PlayOneShot(enemyDie);
    }

    #endregion
}
