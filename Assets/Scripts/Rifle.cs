using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public int mag = 50; //magazines
    private int presentAmmunition;
    public float reloadingTime = 1.3f;
    private bool setReloading = false;

    [Header("Rifle Effects")]
    public ParticleSystem muzzleSpark;
    public GameObject goreEffect;

    public GameObject AmmoOut;

    private void Awake()
    {
        transform.SetParent(hand);
        presentAmmunition = maximumAmmunition;
    }

    private void Start()
    {
        ObjectivesComplete.occurrence.GetObjectivesDone(true, false);
    }

    private void Update()
    {

        if (setReloading)
        {
            if (Input.GetButton("Fire1") || Input.GetButton("Fire2") || (Input.GetButton("Fire1") && Input.GetButton("Fire2")))
            {
                animator.SetBool("Fire", false);
                animator.SetBool("RifleWalk", false);
                animator.SetBool("FireWalk", false);
                AmmoCount.occurrence.UpdateAmmoText(presentAmmunition);
                AmmoCount.occurrence.UpdateMagText(mag);
            }
            animator.SetBool("Walk", false);
            animator.SetBool("Running", false);
            animator.SetBool("RifleAim", false);
            animator.SetBool("IdleAim", false);
            return;
        }

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
        else if(Input.GetButton("Fire1") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) ) //if we fire and walk
        {
            animator.SetBool("Idle", false);
            animator.SetBool("FireWalk", true);
            animator.SetBool("Walk", false);
        }
        else if( Input.GetButton("Fire2") && Input.GetButton("Fire1") || Input.GetButton("Fire2") && ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))) ) //fire and aiming
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
            if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) ||
               (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ||
               (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) ||
               (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                animator.SetBool("Walk", true);
                if (Input.GetButton("Sprint"))
                {
                    animator.SetBool("Walk", false);
                    animator.SetBool("Running", true);
                }
            }
        }
    }

    private void Shoot()
    {
        //check for magazine
        if(mag == 0)
        {
            //show ammo out text
            StartCoroutine(ShowAmmoOut());
            return;
        }

        presentAmmunition--;

        if(presentAmmunition == 0)
        {
            mag--;
        }

        //updating the UI
        AmmoCount.occurrence.UpdateAmmoText(presentAmmunition);
        AmmoCount.occurrence.UpdateMagText(mag);


        if(muzzleSpark != null)
        {
            muzzleSpark.Play();
        }
        RaycastHit hitInfo; // What object ray hits
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);

            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();
            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();

            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDamageOf);
            }
            else if(zombie1 != null)
            {
                zombie1.zombieHitDamage(giveDamageOf);
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectGo, 1f);
            }
            else if(zombie2 != null)
            {
                zombie2.zombieHitDamage(giveDamageOf);
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectGo, 1f);
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
        animator.SetBool("Fire", false);
        //play reload sound
        yield return new WaitForSeconds(reloadingTime);
        animator.SetBool("Reloading", false);
        presentAmmunition = maximumAmmunition;
        player.playerSpeed = 1.9f; //revert
        player.playerSprint = 3; //revert
        setReloading = false; 
    }
    
    IEnumerator ShowAmmoOut()
    {
        AmmoOut.SetActive(true);
        yield return new WaitForSeconds(5f);
        AmmoOut.SetActive(false);
    }
}
