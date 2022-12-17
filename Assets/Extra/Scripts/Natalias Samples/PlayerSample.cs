using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSample : MonoBehaviour
{
    public float moveSpeed;

    public Rigidbody2D rb;
    Vector2 joystickMovement;
    Vector2 keyboardMovement;

    public GameObject ropePrefab;
    GameObject rope;

    public List<GameObject> congaLine = new List<GameObject>();
    public List<GameObject> congaRope = new List<GameObject>();

    bool cycle = false;

    void Start()
    {
        congaLine.Add(this.gameObject);
    }

    void Update()
    {
        keyboardMovement.x = Input.GetAxisRaw("Horizontal");
        keyboardMovement.y = Input.GetAxisRaw("Vertical");

        joystickMovement.x = Input.GetAxisRaw("JS1_hor");
        joystickMovement.y = Input.GetAxisRaw("JS1_ver");

    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + joystickMovement.normalized * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + keyboardMovement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Stuffy" && !cycle)
        {
            if (Input.GetButtonDown("JoystickButton") || Input.GetKeyDown(KeyCode.Return))
            {
                ropeStuffy(congaLine[congaLine.Count - 1], col.gameObject);
                col.tag = "Collected";
                congaLine.Add(col.gameObject);

                moveSpeed += 0.5f;

                cycle = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        cycle = false;
    }

    void ropeStuffy(GameObject front, GameObject end)
    {
        rope = Instantiate(ropePrefab);

        rope.transform.GetChild(0).GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, 0);
        rope.transform.GetChild(0).GetComponent<HingeJoint2D>().connectedBody = front.GetComponent<Rigidbody2D>();
        rope.transform.GetChild(rope.transform.childCount - 1).GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, 0);
        rope.transform.GetChild(rope.transform.childCount - 1).GetComponent<HingeJoint2D>().connectedBody = end.GetComponent<Rigidbody2D>();

        congaRope.Add(rope);
        rope.name = "Rope " + congaRope.Count;
    }

    public void getBrokenJoint(Joint2D joint)
    {
        bool ropeStatus = false;
        bool stuffyStatus = false;
        int ropeIndex = 0;

        for(int i = 0; i < congaRope.Count; i++)
        {
            if(congaRope[i].name == joint.transform.parent.name)
            {
                ropeIndex = i;
                ropeStatus = true;
            }

            if (ropeStatus)
            {
                congaRope[i].name = "broken";
            }
        }

        for (int x = 0; x < congaLine.Count; x++)
        {
            if (x == (ropeIndex+1))
            {
                stuffyStatus = true;
            }

            if (stuffyStatus)
            {
                congaLine[x].name = "stolen";
                moveSpeed -= 0.5f;
            }
        }

        cleaningLists(congaRope, "broken");
        cleaningLists(congaLine, "stolen");
    }

    void cleaningLists(List<GameObject> listaz, string name)
    {
        for(int y = 0; y < listaz.Count; y++)
        {
            if (listaz[y].name == name)
            {
                if (y == 0)
                {
                    listaz.Clear();
                    break;
                }
                else
                {
                    listaz.RemoveAt(y);
                    y = 0;
                }
            }
        }
    }

}
