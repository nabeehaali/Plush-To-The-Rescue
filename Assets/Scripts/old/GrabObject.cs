using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    public GameObject bottomPoint;
    public GameObject storedCollisionA;
    public GameObject storedCollisionP;
    public bool animalGrab = false;
    public bool playerGrab = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Animal" || collision.gameObject.tag == "Collected")
        {
            animalGrab = true;
            //Debug.Log("I have touched the animal!");
            collision.gameObject.transform.parent = bottomPoint.gameObject.transform;
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            storedCollisionA = collision.gameObject;
            //Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), groundCollider.GetComponent<Collider2D>());
            //ignore gound collider
            //disable movement controls
        }
        else if (collision.gameObject.tag == "Player")
        {
            playerGrab = true;
            //Debug.Log("I have touched the animal!");
            collision.gameObject.transform.parent = bottomPoint.gameObject.transform;
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            storedCollisionP = collision.gameObject;
            //Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), groundCollider.GetComponent<Collider2D>());
            //ignore gound collider
            //disable movement controls
            //collision.gameObject.GetComponent<PlayerMove>().enabled = false;
            this.transform.parent.transform.parent.GetComponent<ClawMovement>().caughtStuffy = true;

        }
    }
}
