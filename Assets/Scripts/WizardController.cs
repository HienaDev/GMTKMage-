using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour
{

    public GameObject pointer1;
    public GameObject pointer2;

    public GameObject bullet;

    LineRenderer line;

    GameObject lastbullet = null;

    Vector3 dragAnchor;
    Vector3 dragPoint;

    // Start is called before the first frame update
    void Start()
    {
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
            if(lastbullet != null)
            {
                Destroy(lastbullet);
            }

            Vector3 momentum = finishDrag();
            GameObject shot = Instantiate(bullet, this.transform);
            shot.transform.position = dragAnchor;
            Rigidbody2D shotBody = shot.GetComponent<Rigidbody2D>();
            shotBody.velocity = momentum;
            lastbullet = shot;

        }
        proccessPointer(5);
    }

    void proccessPointer(float intensity)
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        pointer1.transform.position = mouseWorld;
        pointer2.transform.position = mouseWorld;
        pointer1.transform.Rotate(new Vector3(0, 0, 1), Time.deltaTime * 10 * intensity);
        pointer2.transform.Rotate(new Vector3(0, 0, -1), Time.deltaTime * 15 * intensity);
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
}
