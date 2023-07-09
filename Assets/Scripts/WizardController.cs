using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour
{

    [SerializeField] private List<GameObject> traps;
    private int currTrap = 0;

    GameObject activePointer;
    GameObject activeTrap;

    Vector3 lastMouse;

    public bool staggered = false;

    //public float Mana { get; private set; }
    float ManaRegen = 0.5f;
    float ManaBaseline = 0.3f;

    public float Mana;
    public float MaxMana;
    float ManaStart;
    float ManaSpent;

    int friends = 0;

    LineRenderer line;

    Vector3 dragAnchor;
    Vector3 dragPoint;

    public bool CanCast { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        swapTrap();
        lastMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Mana = 1f;
        MaxMana = 1f;

        ManaStart = 1f;
        ManaSpent = 0f;

        CanCast = true;

        line = this.GetComponent<LineRenderer>();
        line.enabled = false;
    }

    public void manaUpgrade()
    {
        MaxMana += 0.5f;
    }

    public void friendUpgrade() {
        friends += 1;
    }

    // Update is called once per frame
    void Update()
    {
        processMana();

        if (Input.GetMouseButtonDown(0))
        {
            enterDrag();
        }

        if (Input.GetMouseButton(0))
        {
            proccessDrag();
        }

        if (Input.GetMouseButtonUp(0) && CanCast)
        {

            Vector3 momentum = finishDrag();
            GameObject trap = Instantiate(activeTrap, this.transform);
            Trap controller = trap.GetComponent<Trap>();

            if (ManaStart < ManaBaseline) 
            {
                Destroy(trap);
            }
            else
            {
                for(int i = 0; i<friends; i++)
                {
                    GameObject frin = Instantiate(activeTrap, this.transform);

                    //Transform spriteFrin = frin.transform.Find("Sprite");
                    //SpriteRenderer sprite = spriteFrin.gameObject.GetComponent<SpriteRenderer>();
                    //sprite.color = Color.yellow;

                    //SpriteRenderer[] sprites = frin.GetComponentsInChildren<SpriteRenderer>();
                    //foreach (SpriteRenderer sprite in sprites)
                    //{
                    //    sprite.color = new Color(1f, 0.5f, 0f);
                    //}

                    Trap frinControl = frin.GetComponent<Trap>();
                    Vector3 randpoint = Random.onUnitSphere;
                    randpoint.z = dragAnchor.z;
                    Vector3 friendpoint = dragAnchor + (randpoint * 10 * friends);
                    frinControl.fireSpell(friendpoint , dragPoint - friendpoint, 0.25f*ManaSpent, true);
                }
                controller.fireSpell(dragAnchor, momentum, ManaSpent, false);
            }
            
            
        }

        proccessPointer();
    }

    void proccessPointer()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    
        mouseWorld.z = 0;
        activePointer.transform.position = mouseWorld;
        if (Input.GetMouseButton(0))
        {
            activePointer.transform.position = dragAnchor;
            activePointer.transform.up = dragAnchor-mouseWorld;
            activePointer.transform.localScale = new Vector3(0.5f, 0.5f, 1f) * (1 + ManaSpent);
        }
        else
        {
            activePointer.transform.position = mouseWorld;
            activePointer.transform.localScale = new Vector3(0.5f, 0.5f, 1f) * 1;
            if(Vector3.Distance(lastMouse, mouseWorld) > 10)
            {
                activePointer.transform.up = lastMouse - mouseWorld;
                lastMouse = mouseWorld;
            }
        }
    }

    void processMana()
    {

        if (Input.GetMouseButtonDown(0))
        {
            ManaStart = Mana;
            ManaSpent = ManaBaseline;
            return;
        }

        if (ManaStart < ManaBaseline)
        {
            Mana += Time.deltaTime * ManaRegen;
            Mana = Mathf.Clamp(Mana, 0f, MaxMana);
            return;
        }

        if (Input.GetMouseButton(0))
        {
            ManaSpent += Time.deltaTime;
            if (ManaSpent > ManaStart)
            {
                ManaSpent = ManaStart;
            }
            Mana = ManaStart - ManaSpent;
        }
        else
        {
            Mana += Time.deltaTime * ManaRegen;
            Mana = Mathf.Clamp(Mana, 0f, MaxMana);
        }        
    }

    void enterDrag()
    {
        line.enabled = true;
        dragAnchor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragAnchor.z = 0;
        line.SetPosition(0, dragAnchor);
    }

    void proccessDrag()
    {
        dragPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (staggered)
        {
            dragPoint += new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
        }
        dragPoint.z = 0;
        
        line.SetPosition(1, dragPoint + new Vector3(Random.value*ManaSpent, Random.value*ManaSpent, 0));
        activePointer.transform.localScale = new Vector3(0.5f, 0.5f, 1f) * (1 + ManaSpent);
        line.startWidth = ManaSpent*2;
        line.endWidth = ManaSpent*2;
    }

    Vector3 finishDrag()
    {
        line.enabled = false;
        if (staggered)
        {
            dragPoint += new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0);
        }
        dragPoint.z = 0;

        return dragPoint-dragAnchor;
    }

    void swapTrap()
    {
        currTrap++;
        if(currTrap >= traps.Count)
        {
            currTrap = 0;
        }
        activeTrap = traps[currTrap];
        Trap trapCont = activeTrap.GetComponent<Trap>();
        swapPointer(trapCont.pointer);
    }

    void swapPointer(GameObject prefab)
    {
        Destroy(activePointer);
        activePointer = Instantiate(prefab, this.transform);
    }

    public void AbleToCast() => StartCoroutine(BackToCasting());

    public void NotAbleToCast() => CanCast = false;

    private IEnumerator BackToCasting()
    {
        yield return new WaitForSeconds(0.1f);

        CanCast = true;
    }
}
