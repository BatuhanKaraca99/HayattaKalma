using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectiveArea : MonoBehaviour
{
    public GameObject indicator;

    public BoxCollider boxcollider2;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            //complete objective 
            if(ObjectivesComplete.occurrence.obj1comp == true)
            {
                ObjectivesComplete.occurrence.GetObjectivesDone(true, true);
                indicator.SetActive(false);
                boxcollider2.enabled = false;
                Destroy(gameObject, 1f);
            }
        }
    }
}
