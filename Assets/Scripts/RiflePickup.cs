using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflePickup : MonoBehaviour
{
    [Header("Rifle's")]
    public GameObject PlayerRifle;
    public GameObject PickupRifle;
    public PlayerPunch playerPunch;

    [Header("Rifle Assign Things")]
    public PlayerScript player;
    private float radius = 2.5f; //interaction zone
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
            nextTimeToPunch = Time.deltaTime + 1F / punchCharge;

            playerPunch.Punch();
        }


        if(Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PlayerRifle.SetActive(true);
                PickupRifle.SetActive(false);
                //sound

                //objective completed
            }
        }
    }
}
