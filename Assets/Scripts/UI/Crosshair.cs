using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {
#if UNITY_STANDALONE_WIN

    [SerializeField] private float sensitivity = 1f; 

#elif UNITY_ANDROID
    [SerializeField] private float sensitivity = 15f;
    [SerializeField] private float distance = 20f;
    [SerializeField] private Joystick joystick;
#endif

    private float x;
    private float y;
    private float currentSensitivity;
    private bool canMove = false;
    private Vector3 screenCenter;
    private Vector3 newPosition;

    private void Start() {
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2 + 100, 0f);
#if UNITY_ANDROID
        newPosition = Vector3.zero;
        currentSensitivity = PlayerPrefs.GetFloat("AimingSensitivity", 20f);

#else
        Cursor.visible = false;
#endif
    }

    void Update() {
        Move();

#if UNITY_STANDALONE_WIN

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.visible = true;
        }
#endif
    }

    private void Move() {

#if UNITY_ANDROID
        if (canMove) {

            newPosition.x = joystick.Horizontal * currentSensitivity;
            newPosition.y = joystick.Vertical * currentSensitivity;

            transform.position = screenCenter + (newPosition.normalized * distance);
        } else {
            MoveCrosshairToCenter();
        }


#else
        Vector3 mousePosition = Input.mousePosition;

        mousePosition.x = Mathf.Clamp(mousePosition.x, 0, Screen.width);
        mousePosition.y = Mathf.Clamp(mousePosition.y, 0, Screen.height);

        transform.position = mousePosition;
#endif
    }

    public void EnableMove() {
        canMove = true;
    }

    public void DisableMove() {
        canMove = false;
    }

    private void MoveCrosshairToCenter() {

        transform.position = Vector3.Lerp(transform.position, screenCenter, Time.deltaTime * 6);
    }
}
