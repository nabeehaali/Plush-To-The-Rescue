using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeBin : MonoBehaviour
{
    Vector2 startPt;
    Vector2 endPt;

    float speed = 0.3f;
    bool activated = false;

    void Start()
    {
        startPt = transform.position;
        endPt = new Vector2(startPt.x, -2f);
    }

    void Update()
    {
        if (activated)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPt, step);
        }
        else
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, startPt, step);
        }
    }

    public void moveDown()
    {
        activated = true;
    }

    public void moveUp()
    {
        activated = false;
    }
}
