using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

    [SerializeField] float sensitivity = 1f; // Check later


    private void Start() {
        Cursor.visible = false;
    }

    void Update() {
        Move();

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.visible = true;
        }
    }

    private void Move() {

        Vector3 mousePosition = Input.mousePosition;

        mousePosition.x = Mathf.Clamp(mousePosition.x, 0, Screen.width);
        mousePosition.y = Mathf.Clamp(mousePosition.y, 0, Screen.height);

        transform.position = mousePosition;
    }
}
