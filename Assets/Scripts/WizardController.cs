using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour
{

    public GameObject lightningPointer;

    [SerializeField] private List<GameObject> pointers;
    private int currPointer = 0;

    GameObject activePointer;

    public GameObject trap;

    LineRenderer line;

    Vector3 dragAnchor;
    Vector3 dragPoint;

    // Start is called before the first frame update
    void Start()
    {
        currPointer++;
        if(currPointer == pointers.Count)
        {
            currPointer = 0;
        }
        swapPointer(pointers[currPointer]);

        line = this.GetComponent<LineRenderer>();
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
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
            GameObject activeTrap = Instantiate(trap, this.transform);
            LightningTrap controller = activeTrap.GetComponent<LightningTrap>();
            controller.fireSpell(dragAnchor, momentum);

        }
        proccessPointer();
    }

    void proccessPointer()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        activePointer.transform.position = mouseWorld;
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
        dragPoint.z = 0;
        line.SetPosition(1, dragPoint);
    }

    Vector3 finishDrag()
    {
        line.enabled = false;
        return dragPoint-dragAnchor;
    }

    void swapPointer(GameObject prefab)
    {
        Destroy(activePointer);
        activePointer = Instantiate(prefab, this.transform);
    }
}
