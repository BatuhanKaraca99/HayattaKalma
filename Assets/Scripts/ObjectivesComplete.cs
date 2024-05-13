using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectivesComplete : MonoBehaviour
{
    [Header("Objectives to Complete")]
    public TMP_Text objective1;
    public TMP_Text objective2;
    public TMP_Text objective3;

    public bool obj1comp = false;
    public bool obj2comp = false;
    public bool obj3comp = false;

    public static ObjectivesComplete occurrence;

    private void Awake()
    {
        occurrence = this;
    }

    public void GetObjectivesDone(bool obj1 = false, bool obj2 = false, bool obj3 = false)
    {
        if(obj1 == true && obj2 == false)
        {
            objective1.text = "1.1: GOREV TAMAMLANDI";
            objective1.color = Color.grey;
            obj1comp = true;
        }
        if(obj1comp == true && obj2 == true)
        {
            objective2.text = "1.2: GOREV TAMAMLANDI";
            objective2.color = Color.grey;
            obj2comp = true;
        }
        if (obj1comp == true && obj2comp == true && obj3 == true)
        {
            objective3.text = "1.3: GOREV TAMAMLANDI";
            objective3.color = Color.grey;
            obj3comp = true;
        }
    }
}
