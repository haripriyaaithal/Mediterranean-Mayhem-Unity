using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    [SerializeField] private Transform bulletFirePosition;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float fireRate = 0.6f;

    [SerializeField] GameObject crosshair;
    [SerializeField] Player player;

    private float currentTime = 0f;
    private Vector3 firingDirection;

    // Update is called once per frame
    void Update() {

        AimAtCrosshair();

        if (Input.GetMouseButton(0)) {
            player.AnimatingShooting(true);

            Fire();
        } else {
            player.AnimatingShooting(false);

        }
        currentTime += Time.deltaTime;
    }

    private void AimAtCrosshair() {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
        float hitInfo;

        if (plane.Raycast(ray, out hitInfo)) {

            Debug.DrawRay(Camera.main.transform.position, ray.GetPoint(hitInfo), Color.blue);

            firingDirection = Vector3.ProjectOnPlane(ray.GetPoint(hitInfo) - player.GetPlayerPosition(), plane.normal);
            
        }
    }

    private void Fire() {
        if (currentTime >= fireRate) {
            currentTime = 0f;
            // USE OBJECT POOLING
            // Fire a bullet
            Bullet bullet = Instantiate(bulletPrefab, bulletFirePosition.position, player.transform.rotation);
            bullet.SetFiringDirection(firingDirection.normalized);

        } 

    }
}