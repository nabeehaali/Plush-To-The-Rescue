using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawSimulation : MonoBehaviour
{
    public GameObject player;

    public List<GameObject> verticies;
    Vector2 currentPos;
    Vector2 targetPos;
    int probability;

    void Start()
    {
        probability = 10;
        targetPos = new Vector2(currentPos.x, currentPos.y);
        StartCoroutine(createPoint());

        Debug.Log(player.transform.position.x);
    }

    void Update()
    {
        currentPos = gameObject.transform.position;
        float step = 1 * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, step);

        if(currentPos == targetPos)
        {
            //claw down
            Debug.Log("I have hit my target!");
        }
    }

    IEnumerator createPoint()
    {
        while(true)
        {
            yield return new WaitForSeconds(5);
            probability += 5;
            int i = Random.Range(0, 100);
            
            if (probability > 90)
            {
                probability = 90;
            }

            if (i >= 0 && i <= probability)
            {
                createTargetedPoint();
            }
            else if (i >= (probability + 1) && i <= 100)
            {
                createRandomPoint();
            }
        }
      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            //claw down
            Debug.Log("I have touched the border!");
        }
    }

    void createRandomPoint()
    {
        targetPos = new Vector2(Random.Range(verticies[0].transform.position.x, verticies[1].transform.position.x), Random.Range(verticies[2].transform.position.y, verticies[3].transform.position.y));
        Debug.Log("Target is random and is located at:" + targetPos);
    }

    void createTargetedPoint()
    {
        targetPos = new Vector2(player.transform.position.x, Random.Range(verticies[2].transform.position.y, verticies[3].transform.position.y));
        Debug.Log("Target has been made using player position and it is located at: " + targetPos);
    }
}

/*How this all works:
 * on start, the claw will be positioned at where it is in the scene
 * coroutine is called where every 5 seconds, a random point is generated within the bounding rectanlge of the roof
 * the claw will then move towards that generated point
 * conditions will occur when it hits the dedicated border (colission) of the roof
 * conditions will occur when the claw is positioned at the target point
 */

/*To-Do
 * create a claw down function (take clawBar gameobject and expand the scale.y for a certain amount while animating the claw base)
 * if the claw base collides with something, run grabbing function
 * create a targetting player function = DONE!!
 * get player x position, randomize y based on bounding box (will have to fix this later) = DONE!! (FOR NOW)
 * increase probability of generating postion based on player over time = DONE!!
 * intiial probability is 10%, increase by 5 everytime a point is generated, max out at 90% = DONE!!
 * generate random value between 1-100 = DONE!!
 * if it's between 0 and than the probability assignmed, then generate point based on player position = DONE!!
 * if it's bigger than probability+1 and less than 100, then generate point out of random = DONE!!
 */
