using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    int animalsCollected;
    Vector2 randomPoint;
    public GameObject animalPrefab;

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
        animalsCollected = 0;

        PlayerPrefs.SetInt("Score", 0);

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
        if (col.tag == "Claw")
        {
            col.transform.parent.transform.parent.GetComponent<ClawMovement>().caughtStuffy = true;
            col.transform.parent.transform.parent.GetComponent<ClawMovement>().gameover = true;
            this.transform.position = col.transform.position;
            Destroy(this.GetComponent<CircleCollider2D>());
        }

        if (col.tag == "Animal" && !cycle)
        {
            if (Input.GetButtonDown("JoystickButton") || Input.GetKeyDown(KeyCode.Return))
            {
                ropeStuffy(congaLine[congaLine.Count - 1], col.gameObject);
                col.tag = "Collected";
                congaLine.Add(col.gameObject);

                col.name = "Stuffy " + congaLine.Count;

                moveSpeed += 0.1f;

                cycle = true;

                animalsCollected++;

                if (animalsCollected >= 2)
                {
                    StartCoroutine(createAnimal());
                }
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
        rope.transform.position = this.transform.position;

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

        for (int i = 0; i < congaRope.Count; i++)
        {
            if (congaRope[i].name == joint.transform.parent.name)
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
            if (x == (ropeIndex + 1))
            {
                stuffyStatus = true;
            }

            if (stuffyStatus)
            {
                congaLine[x].name = "stolen";
                moveSpeed -= 0.2f;
            }
        }

        cleaningLists(congaRope, "broken");
        cleaningLists(congaLine, "stolen");
    }

    void cleaningLists(List<GameObject> listaz, string name)
    {
        for (int y = 0; y < listaz.Count; y++)
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

    public void setPlayerScore()
    {
        if ((congaLine.Count-1) > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", (congaLine.Count - 1));
            PlayerPrefs.Save();
        }

        PlayerPrefs.SetInt("Score", (congaLine.Count - 1));

    }

    IEnumerator createAnimal()
    {
        int i = Random.Range(0, 6);
        yield return new WaitForSeconds(i);
        randomPoint = new Vector2(Random.Range(-1.95f, 2f), Random.Range(-3.57f, -5.27f));
        Instantiate(animalPrefab, randomPoint, Quaternion.identity);
    }

    public void stuffyCaughtByClaw(string name)
    {
        Debug.Log(name + " was caught");

        int index = 0;

        for (int t = 0; t < congaLine.Count; t++)
        {
            if (congaLine[t].name == name)
            {
                Debug.Log("found it");
                index = t;
            }
        }

        for (int v = 0; v < congaRope.Count; v++)
        {
            if (v == (index-1))
            {
                Debug.Log(congaRope[v].name + " breaking " + congaRope[v].transform.GetChild(1).name);
                congaRope[v].transform.GetChild(1).GetComponent<HingeJoint2D>().breakForce = 1;
            }
        }
    }

}
