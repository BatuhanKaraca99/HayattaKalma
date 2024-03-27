using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Rifle Things")]
    public Camera cam;

    public float giveDamageOf = 10f; // To Zombie
    public float shootingRange = 100f;
    public float fireCharge = 15f; // 15 times fire
    private float nextTimeToShoot = 0f;

    [Header("Rifle Effects")]
    public ParticleSystem muzzleSpark;

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.deltaTime + 1f / fireCharge;
            Shoot();
        }
    }

    private void Shoot()
    {
        if(muzzleSpark != null)
        {
            muzzleSpark.Play();
        }
        RaycastHit hitInfo; // What object ray hits
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);

            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();

            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDamageOf);
            }
        }
    }

}
