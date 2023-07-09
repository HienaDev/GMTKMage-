using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    // Start is called before the first frame update
    private WizardController checkMana;
    private float mana;
    private float maxMana;

    private Image manaBar;

    private TMP_Text text;

    void Start()
    {
        if (checkMana == null)
        {
            checkMana = FindObjectOfType<WizardController>();
        }
        manaBar = GetComponent<Image>();

        text = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        mana = checkMana.Mana;
        maxMana = checkMana.MaxMana;

 
        manaBar.fillAmount = mana / maxMana;

        text.text = ((int)(mana * 100)).ToString() + "/" + (maxMana * 100).ToString();

    }
}
