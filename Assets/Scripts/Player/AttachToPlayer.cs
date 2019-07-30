using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToPlayer : MonoBehaviour {
    
    [SerializeField] private Transform hand;
        
    // Update is called once per frame
    void LateUpdate() {
        if (hand != null) {
            transform.position = hand.position;
            transform.rotation = hand.rotation;
        }
    }
}
