using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Rifle Things")]
    public Camera cam;

    public float giveDamageOf = 10f; // To Zombie
    public float shootingRange = 100f;

    [Header("Rifle Effects")]
    public ParticleSystem muzzleSpark;

    private void LateUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
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
