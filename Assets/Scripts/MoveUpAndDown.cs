using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{

    [SerializeField] private float maxDistance;
    private float distance = 0f;
    [SerializeField] private float floatingRatio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + floatingRatio, gameObject.transform.position.z);

        if (distance < maxDistance && floatingRatio >= 0)
        {
            distance += floatingRatio;
        }
        else if (distance >= maxDistance && floatingRatio >= 0)
        {
            floatingRatio *= -1;
            maxDistance *= -1;
        } else if (distance > maxDistance && floatingRatio <= 0)
        {
            distance += floatingRatio;
        }
        else if (distance <= maxDistance && floatingRatio <= 0)
        {
            floatingRatio *= -1;
            maxDistance *= -1;
        }

    }
}
