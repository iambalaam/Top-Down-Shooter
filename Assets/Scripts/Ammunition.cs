using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ammunition : MonoBehaviour
{
    private string MONO_SPACING = "0.5em";
    private TextMeshProUGUI text;
    public GunController gun;


    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    string SytleText(string text)
    {
        // Is there a nicer text templating system?
        return "<mspace=" + MONO_SPACING + ">" + text + "</mspace>";
    }

    void Update()
    {
        int ammunition = gun.GetAmmunition();
        string numbers = this.SytleText(ammunition.ToString());
        text.SetText(numbers);
    }
}
