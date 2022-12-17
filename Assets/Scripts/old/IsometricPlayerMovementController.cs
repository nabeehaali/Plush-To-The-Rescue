using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour
{

    public float movementSpeed = 1f;
    private Vector2 target;
    private Vector2 position;
    private int probabilityCap;
    IsometricCharacterRenderer isoRenderer;

    Rigidbody2D rbody;

    public GameObject player;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
    }

    private void Start()
    {
        target = new Vector2(0f, 3.18f);
        position = gameObject.transform.position;
        probabilityCap = 10;
        StartCoroutine(startMoving());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        isoRenderer.SetDirection(movement);
        rbody.MovePosition(newPos);

        float step = movementSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target, step);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            Debug.Log("I've touched the edge of the machine");
            //drop the claw
        }
    }
    IEnumerator startMoving()
    {
        while (true)
        {
            //change value in waitforSeconds to however long it takes to do down animation + movement
            yield return new WaitForSeconds(10);
            Debug.Log(position);
            probabilityCap += 5;

            int i = Random.Range(0, 100);
            Debug.Log(i);
            
            if (probabilityCap > 90)
            {
                probabilityCap = 90;
            }

            //Debug.Log(probabilityCap);

            if (i >= 0 && i <= probabilityCap)
            {
                positionByPlayer();
            }
            else if (i >= (probabilityCap+1) && i <= 100)
            {
                positionByClaw();
            }
            //target = new Vector2(Random.Range(-6.2f, 6.2f), Random.Range(-0.5f, 3.5f));
            //Debug.Log(target);
        }
        
    }

    void positionByPlayer()
    {
        //select player gameobject, get its position, set target of the claw to be same x value, but random y
        target = new Vector2(player.transform.position.x, Random.Range(-0.5f, 3.5f));

        //check if point is inside triangles, if not, generate new point
        Debug.Log("Target has been made using player position and it is located at: " + target);
    }

    void positionByClaw()
    {
        target = new Vector2(Random.Range(-6.2f, 6.2f), Random.Range(-0.5f, 3.5f));
        Debug.Log("Target is random and is located at: " + target);
    }
}
