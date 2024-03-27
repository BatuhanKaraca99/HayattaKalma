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
    public PlayerScript player;

    [Header("Rifle Ammunition and Shooting")]
    private int maximumAmmunition = 32;
    public int mag = 10; //magazines
    private int presentAmmunition;
    public float reloadingTime = 1.3f;
    private bool setReloading = false;


    [Header("Rifle Effects")]
    public ParticleSystem muzzleSpark;

    private void Awake()
    {
        presentAmmunition = maximumAmmunition;
    }

    private void Update()
    {
        if (setReloading)
            return;

        if(presentAmmunition <= 0 )
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.deltaTime + 1f / fireCharge;
            Shoot();
        }
    }

    private void Shoot()
    {
        //check for magazine
        if(mag == 0)
        {
            //show ammo out text
            return;
        }

        presentAmmunition--;

        if(presentAmmunition == 0)
        {
            mag--;
        }

        //updating the UI


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

    IEnumerator Reload()
    {
        player.playerSpeed = 0f;
        player.playerSprint = 0f;
        setReloading = true;
        Debug.Log("Reloading...");
        //play anim
        //play reload sound
        yield return new WaitForSeconds(reloadingTime);
        //play anim
        presentAmmunition = maximumAmmunition;
        player.playerSpeed = 1.9f; //revert
        player.playerSprint = 3; //revert
        setReloading = false; 
    }
}
