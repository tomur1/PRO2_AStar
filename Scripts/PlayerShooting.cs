using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public float shootingDelay;
    public GameObject bulletPrefab;
    public GameObject shootingPlace;

    private bool wantsToFire;
    private bool onCooldown;
    
    void Update()
    {
        wantsToFire = Input.GetButton("Fire1");

        if (wantsToFire && !onCooldown)
        {
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        Instantiate(bulletPrefab, shootingPlace.transform.position, transform.rotation);
        StartCoroutine(CooldownAfterFire());
    }

    IEnumerator CooldownAfterFire()
    {
        onCooldown = true;
        yield return new WaitForSeconds(shootingDelay);
        onCooldown = false;
    }
}