using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [Header("Player Punch Variables")]
    public Camera cam;
    public float giveDamageOf = 10f;
    public float punchingRange = 7f;

    public void Punch()
    {
        RaycastHit hitInfo;

        if(Physics.Raycast(cam.transform.position,cam.transform.forward, out hitInfo, punchingRange))
        {
            Debug.Log(hitInfo.transform.name);

            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();

            if(objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDamageOf);
            }
        }
    }
}
