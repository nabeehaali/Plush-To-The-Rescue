using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlushyScript : MonoBehaviour
{
    // Caught by the claw Vars
    public bool follow = false;
    public bool yesyou = false;
    public bool switchUp = false;
    public bool toDeath = false;

    // Type of animal (e.g. Cow, Chicken, Whale, etc.)
    public string animalType;

    // Time to zoom to top of machine after claw drags plushy up
    float clawSpeedTime = 5;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(11, 11, true);                       // Caught plushies ignore one another
    }

    void FixedUpdate()
    {
        // Activating particle system for plushies within the rescue line
        if (this.gameObject.layer == 6)
        {
            if (GetComponent<Rigidbody2D>().velocity.x > 0.3f || GetComponent<Rigidbody2D>().velocity.y > 0.3f)
            {
                transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().startLifetime = 0.7f;
            }
            else
            {
                transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().startLifetime = 0f;
            }
        }
        else
        {
            transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().startLifetime = 0f;
        }
        
        
        if (this.tag == "StuffyClaw" && GameObject.Find("ClawMain").GetComponent<ClawMovement>().destroyStuffy)         // Caught plushy reached top of machine
        {
            toDeath = true;
            yesyou = false;
        }
        else if (yesyou)                                                                                                // Caught plushy still being dragged upwards
        {
            if (this.tag == "StuffyClaw")
            {
                Vector2 towardsOther = (GameObject.Find("ClawPosTracker").transform.position - transform.parent.position).normalized;
                transform.parent.GetComponent<Rigidbody2D>().MovePosition(new Vector2(transform.parent.position.x, transform.parent.position.y) + (towardsOther * (GameObject.Find("ClawMain").GetComponent<ClawMovement>().clawSpeed + 0.2f) * Time.deltaTime));
            }
        }

        if (toDeath)                                                                                                    // Dragging plushy up and destroying
        {
            Destroy(transform.GetComponent<HingeJoint2D>());
            Physics2D.IgnoreLayerCollision(11, 15, true);
            GetComponent<Rigidbody2D>().MovePosition(Vector3.Lerp(transform.position, GameObject.Find("TopOfMachine").transform.position, 0.02f));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Caught by the claw
        if (other.gameObject.tag == "Claw") 
        {
            if (!other.transform.parent.transform.parent.GetComponent<ClawMovement>().caughtStuffy)                                     // Not caught yet
            {
                // Updating plushy stats
                this.gameObject.layer = 11;
                transform.parent.GetComponent<HingeJoint2D>().anchor = new Vector2(0, 0);
                transform.parent.GetComponent<HingeJoint2D>().connectedBody = GameObject.Find("ClawPosTracker").GetComponent<Rigidbody2D>();
                transform.GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;
                transform.GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, 0);
                transform.parent.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

                // Collected plushy = need to remove from rescue line
                if (this.tag == "Collected")
                {
                    this.gameObject.layer = 11;
                    GameObject.Find("Player").GetComponent<MainPlayerScript>().removeStuffies(this.name);
                }
                // Not a collected plushy
                else if(this.tag == "Animal")
                {
                    this.gameObject.layer = 17;
                }

                this.tag = "StuffyClaw";
                yesyou = true;
                other.transform.parent.transform.parent.GetComponent<ClawMovement>().caughtStuffy = true;
            }
        }

        // Collided with top of Claw Machine
        if (other.gameObject.tag == "ClawMachine")
        {
            destroyStuffy();
        }

        // Collided with top of Claw Machine
        if (other.gameObject.name == "TopOfMachine")
        {
            toDeath = true;
            destroyStuffy();
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Claw" && this.tag == "StuffyClaw")
        {
            col.transform.parent.transform.parent.GetComponent<ClawMovement>().caughtStuffy = true;

            if (col.transform.parent.transform.parent.GetComponent<ClawMovement>().destroyStuffy)
            {
                col.transform.parent.transform.parent.GetComponent<ClawMovement>().caughtStuffy = false;
            }
        }
    }

    // Destroy plushies thrown in prize bin
    public void thrownStuffyDestroy()
    {
        StartCoroutine("destroyObj", 6f);
    }

    // Destroy plushy dragged away by the claw
    public void destroyStuffy()
    {
        StartCoroutine("destroyObj", clawSpeedTime - GameObject.Find("ClawMain").GetComponent<ClawMovement>().clawSpeed);
    }

    IEnumerator destroyObj(int value)
    {
        yield return new WaitForSeconds(value);

        if (this.gameObject.layer == 11)                                             // Caught plushy within rescue line
        {
            Destroy(transform.parent.gameObject);
        }
        else if (this.gameObject.layer == 17)                                        // Caught plushy not in rescue line
        {
            Destroy(transform.parent.parent.gameObject);
        }
        else if (this.gameObject.layer == 8)                                        // Thrown plushy
        {
            Destroy(transform.gameObject);
        }

        GameObject.Find("ClawMain").GetComponent<ClawMovement>().caughtStuffy = false;
        GameObject.Find("ClawMain").GetComponent<ClawMovement>().destroyStuffy = false;

        Destroy(this.gameObject);
    }
}
