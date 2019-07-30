using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour {

    [SerializeField] float rotationSpeed = 2f;

    // Update is called once per frame
    void LateUpdate() {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
