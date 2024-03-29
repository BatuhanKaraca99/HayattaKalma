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
    public Animator animator;
    public PlayerScript player;
    public Transform hand;

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
        transform.SetParent(hand);
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

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot) //if we fire
        {
            animator.SetBool("Fire", true);
            animator.SetBool("Idle", false);
            nextTimeToShoot = Time.deltaTime + 1f / fireCharge;
            Shoot();
        }
        else if(Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) //if we fire and walk
        {
            animator.SetBool("Idle", false);
            animator.SetBool("FireWalk", true);
        }
        else if(Input.GetButton("Fire2") && Input.GetButton("Fire1")) //fire and aiming
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("FireWalk", true);
            animator.SetBool("Walk", true);
            animator.SetBool("Reloading", false);
        }
        else
        {
            animator.SetBool("Fire", false);
            animator.SetBool("Idle", true);
            animator.SetBool("FireWalk", false);
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
        animator.SetBool("Reloading", true);
        //play reload sound
        yield return new WaitForSeconds(reloadingTime);
        animator.SetBool("Reloading", false);
        presentAmmunition = maximumAmmunition;
        player.playerSpeed = 1.9f; //revert
        player.playerSprint = 3; //revert
        setReloading = false; 
    }
}
