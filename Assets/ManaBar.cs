using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    // Start is called before the first frame update
    private WizardController checkMana;
    private float mana;

    private Image manaBar;
    
    void Start()
    {
        if (checkMana == null)
        {
            checkMana = FindObjectOfType<WizardController>();
        }
        manaBar = GetComponent<Image>();    

    }

    // Update is called once per frame
    void Update()
    {
        mana = checkMana.Mana;

 
        manaBar.fillAmount = mana / 1f;

        Debug.Log(mana);
    }
}
