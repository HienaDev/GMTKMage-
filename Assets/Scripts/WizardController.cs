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

    float mana = 1f;
    float manaRegen = 1f;
    float manaBaseline = 0.2f;

    float manaStart = 0f;
    float manaSpent = 0f;



    LineRenderer line;

    Vector3 dragAnchor;
    Vector3 dragPoint;

    // Start is called before the first frame update
    void Start()
    {
        swapTrap();
        lastMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

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

            if(manaSpent > manaStart)
            {
                Destroy(trap);
            }
            else
            {
                controller.fireSpell(dragAnchor, momentum, manaSpent);
            }
        }

        if (mana <= 1f)
        {
            mana += Time.deltaTime/10f*manaRegen;
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
            float manaSpent = Mathf.Clamp(Time.time - startHold, 0f, 1f);
            activePointer.transform.position = dragAnchor;
            activePointer.transform.up = dragAnchor-mouseWorld;
            activePointer.transform.localScale = new Vector3(0.5f, 0.5f, 1f) * (1 + manaSpent);
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
            mana += Time.deltaTime * 10f * manaRegen;
        }
        else
        {
            manaSpent = Mathf.Clamp(Time.time - startHold + manaBaseline, 0f, 1f);
            if (manaSpent > manaStart)
            {
                manaSpent = manaStart;
            }
            mana = manaStart - manaSpent;
        }        
    }

    void enterDrag()
    {
        line.enabled = true;
        dragAnchor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragAnchor.z = 0;
        line.SetPosition(0, dragAnchor);

        startHold = Time.time;
        manaStart = mana;

    }

    void proccessDrag()
    {
        dragPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragPoint.z = 0;
        
        line.SetPosition(1, dragPoint + new Vector3(Random.value*manaSpent, Random.value*manaSpent, 0));
        activePointer.transform.localScale = new Vector3(0.5f, 0.5f, 1f) * (1 + manaSpent);
        line.startWidth = manaSpent*2;
        line.endWidth = manaSpent*2;
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
