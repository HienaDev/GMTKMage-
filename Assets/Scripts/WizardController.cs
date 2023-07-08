using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour
{

    [SerializeField] private List<GameObject> traps;
    private int currTrap = 0;

    GameObject activePointer;
    GameObject activeTrap;

    float startHold;
    float timeHeld;

    Vector3 lastMouse;

    public float Mana { get; private set; }
    float ManaRegen = 1f;
    float ManaBaseline = 0.2f;

    float ManaStart = 0f;
    float ManaSpent = 0f;



    LineRenderer line;

    Vector3 dragAnchor;
    Vector3 dragPoint;

    // Start is called before the first frame update
    void Start()
    {
        swapTrap();
        lastMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Mana = 1;

        line = this.GetComponent<LineRenderer>();
        line.enabled = false;
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

        if (Input.GetMouseButtonUp(0))
        {

            Vector3 momentum = finishDrag();
            GameObject trap = Instantiate(activeTrap, this.transform);
            Trap controller = trap.GetComponent<Trap>();

            if(ManaSpent > ManaStart)
            {
                Destroy(trap);
            }
            else
            {
                controller.fireSpell(dragAnchor, momentum, ManaSpent);
            }
        }

        if (Mana <= 1f)
        {
            Mana += Time.deltaTime/10f*ManaRegen;
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
            float ManaSpent = Mathf.Clamp(Time.time - startHold, 0f, 1f);
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
        if (!Input.GetMouseButton(0))
        {
            Mana += Time.deltaTime * 10f * ManaRegen;
        }
        else
        {
            ManaSpent = Mathf.Clamp(Time.time - startHold + ManaBaseline, 0f, 1f);
            if (ManaSpent > ManaStart)
            {
                ManaSpent = ManaStart;
            }
            Mana = ManaStart - ManaSpent;
        }        
    }

    void enterDrag()
    {
        line.enabled = true;
        dragAnchor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragAnchor.z = 0;
        line.SetPosition(0, dragAnchor);

        startHold = Time.time;
        ManaStart = Mana;

    }

    void proccessDrag()
    {
        dragPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragPoint.z = 0;
        
        line.SetPosition(1, dragPoint + new Vector3(Random.value*ManaSpent, Random.value*ManaSpent, 0));
        activePointer.transform.localScale = new Vector3(0.5f, 0.5f, 1f) * (1 + ManaSpent);
        line.startWidth = ManaSpent*2;
        line.endWidth = ManaSpent*2;
    }

    Vector3 finishDrag()
    {
        line.enabled = false;
        timeHeld = Time.time;
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
}
