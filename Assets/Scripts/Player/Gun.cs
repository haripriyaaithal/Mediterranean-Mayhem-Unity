using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    [SerializeField] private Transform bulletFirePosition;
    //[SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float fireRate = 0.6f;

    [SerializeField] private GameObject crosshair;
    [SerializeField] private Player player;
    [SerializeField] private Pools pools;
    [SerializeField] private SoundManager soundManager;

    private float currentTime = 0f;
    private Vector3 firingDirection;

    private bool canFire = false;

    // Update is called once per frame
    void Update() {

        if (player) {
            AimAtCrosshair();

            if (Input.GetMouseButton(0)) {

#if UNITY_ANDROID
                if (canFire) {
                    player.AnimatingShooting(true);
                    Fire();
                }
#else
                player.AnimatingShooting(true);
                Fire();
#endif


            } else {
                player.AnimatingShooting(false);

            }
            currentTime += Time.deltaTime;
        } else {
            Destroy(gameObject);
        }
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

            soundManager.PlayerFire();

            // Fire a bullet
            //Bullet bullet = Instantiate(bulletPrefab, bulletFirePosition.position, player.transform.rotation);
            GameObject bullet = pools.GetBullet();
            bullet.transform.position = bulletFirePosition.position;
            bullet.transform.rotation = bulletFirePosition.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().SetFiringDirection(firingDirection.normalized);

        }
    }

#if UNITY_ANDROID
    public void EnableFire() {
        canFire = true;
    }

    public void DisableFire() {
        canFire = false;
    }
#endif

}