using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCollectible : MonoBehaviour
{
    // Start is called before the first frame update

    private WizardController controller;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        controller = FindObjectOfType<WizardController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(DisableSpells());
    }

    private IEnumerator DisableSpells()
    {
        controller.staggered = true;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);

        yield return new WaitForSeconds(5f);

        controller.staggered = false;
        Destroy(gameObject);
 
    }
}
