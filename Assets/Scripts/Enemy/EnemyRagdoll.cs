using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour {

    private WaitForSeconds waitForThreeSecond;

    private bool canRunCoroutine = false;

    // Start is called before the first frame update
    private void Start() {
        if (waitForThreeSecond == null) {
            waitForThreeSecond = new WaitForSeconds(3f);
        }
     
        canRunCoroutine = true;
        OnEnable();
    }

    private void OnEnable() {
        if (canRunCoroutine) {
            StartCoroutine(VanishWithDelay());
        }
    }


    private IEnumerator VanishWithDelay() {
        yield return waitForThreeSecond;
        Destroy(gameObject);
    }
}
