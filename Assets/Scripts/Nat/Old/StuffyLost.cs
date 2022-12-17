using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffyLost : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Claw")
        {
            if (this.tag == "Collected") {
                GameObject.Find("Player").GetComponent<PlayerControls>().stuffyCaughtByClaw(this.name);
            }

            this.tag = "StuffyClaw";

            col.transform.parent.transform.parent.GetComponent<ClawMovement>().caughtStuffy = true;
            this.transform.position = col.transform.position;
            Destroy(this.GetComponent<CircleCollider2D>());

            if (col.transform.parent.transform.parent.GetComponent<ClawMovement>().destroyStuffy)
            {
                col.transform.parent.transform.parent.GetComponent<ClawMovement>().caughtStuffy = false;
                Destroy(this.gameObject);
            }
        }
    }
}
