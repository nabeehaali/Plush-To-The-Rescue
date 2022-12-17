using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveitmoveit : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody rb;
    Vector3 keyboardMovement;

    public float speed = 2f;


    // Update is called once per frame
    void Update()
    {
        keyboardMovement.x = Input.GetAxisRaw("Horizontal");
        keyboardMovement.y = Input.GetAxisRaw("Vertical");

        /*if (keyboardMovement.x > 0.7f || keyboardMovement.x < -0.7f)
        {
            transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().startLifetime = 0.7f;
        }
        else if (keyboardMovement.y > 0.7f || keyboardMovement.y < -0.7f)
        {
            transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().startLifetime = 0.7f;
        }
        else
        {
            transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().startLifetime = 0f;
        }*/
    }
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + keyboardMovement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}