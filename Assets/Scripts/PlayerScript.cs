using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerSpeed = 1.9f;

    [Header("Player Animator and Gravity")]
    public CharacterController cC;

    private void FixedUpdate()
    {
        playerMove();
    }

    void playerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg; //Find angle from x/z(horizontal/vertical)
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f); //Apply transform               
            cC.Move(direction * playerSpeed * Time.deltaTime);
        }
    }
}
