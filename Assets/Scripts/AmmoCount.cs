using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCount : MonoBehaviour
{
    public TMP_Text ammunitionText;
    public TMP_Text magText;

    public static AmmoCount occurrence;

    private void Awake()
    {
        if(occurrence == null)
        {
            occurrence = this;
        }
    }

    public void UpdateAmmoText(int presentAmmunition)
    {
        ammunitionText.text = "Mermi: " + presentAmmunition;
    }

    public void UpdateMagText(int mag)
    {
        magText.text = "Þarjör: " + mag;
    }
}
