using System.Collections;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public InputManager inputManager;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireCooldown = 0.3f;
    private float bulletSpeed = 10f; 
    private float lastFireTime;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            TryFire();
        }
    }

    void TryFire()
    {
        if (Time.time - lastFireTime >= fireCooldown)
        {
            Fire();
            lastFireTime = Time.time;
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = (inputManager.MousePosVec - (Vector2)transform.position).normalized * bulletSpeed;
        Debug.Log(rb.linearVelocity);
    }
}
