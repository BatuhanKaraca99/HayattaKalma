using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflePickup : MonoBehaviour
{
    [Header("Rifle's")]
    public GameObject PlayerRifle;
    public GameObject PickupRifle;
    public PlayerPunch playerPunch;
    private int maximumAmmunition = 32;
    public int mag = 50;

    [Header("Rifle Assign Things")]
    public PlayerScript player;
    private float radius = 2.5f; //interaction zone
    public Animator animator;
    private float nextTimeToPunch = 0; //sleep
    public float punchCharge = 15f;

    private void Awake()
    {
        PlayerRifle.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToPunch)
        {
            animator.SetBool("Punch", true);
            animator.SetBool("Idle", false);

            nextTimeToPunch = Time.deltaTime + 1F / punchCharge;

            playerPunch.Punch();
            player.velocity = Vector3.zero;
            player.playerSpeed = 0;
            player.playerSprint = 0;
        }

        else
        {
            animator.SetBool("Punch", false);
            animator.SetBool("Idle", true);
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                animator.SetBool("Idle", false);
                player.playerSpeed = 1.9f;
                player.playerSprint = 3f;
            }
        }

        if(Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PlayerRifle.SetActive(true);
                PickupRifle.SetActive(false);
                //UI
                AmmoCount.occurrence.UpdateAmmoText(maximumAmmunition);
                AmmoCount.occurrence.UpdateMagText(mag);
                //sound

            }
        }
    }
}
