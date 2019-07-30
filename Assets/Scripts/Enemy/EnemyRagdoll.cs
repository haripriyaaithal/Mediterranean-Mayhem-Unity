using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour {

    //private Pools pools;
    private WaitForSeconds waitForThreeSecond;

    private bool canRunCoroutine = false;

    // Start is called before the first frame update
    private void Start() {
        /*if (pools == null) {
            pools = FindObjectOfType<Pools>();
        }*/
        if (waitForThreeSecond == null) {
            waitForThreeSecond = new WaitForSeconds(3f);
        }
     
        canRunCoroutine = true;
        OnEnable();
    }

    private void OnEnable() {
        if (canRunCoroutine) {
            StartCoroutine(VanishWithAnimation());
        }
    }


    private IEnumerator VanishWithAnimation() {
        yield return waitForThreeSecond;

        /*gameObject.SetActive(false);
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;

        pools.AddRagdollToPool(gameObject);*/
        Destroy(gameObject);
    }
}
