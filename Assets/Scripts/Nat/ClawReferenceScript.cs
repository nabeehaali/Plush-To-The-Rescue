using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawReferenceScript : MonoBehaviour
{
    public GameObject claw;

    // Reference point that follows the claw
    // Chain of plushies will follow this point when being dragged by the claw
    void FixedUpdate()
    {
        Vector2 towardsOther = (claw.transform.position - transform.position).normalized;
        float speed = 5f;
        transform.GetComponent<Rigidbody2D>().MovePosition(new Vector2(transform.position.x, transform.position.y) + (towardsOther * speed * Time.deltaTime));
    }

}
