using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveClaw : MonoBehaviour
{
    public GameObject player;

    private Vector2 current;
    private Vector2 target;
    private float speed = 3f;
    public int minOffset;
    public int maxOffset;
    int probability;

    void Start()
    {
        probability = 10;
        target = new Vector2(0, 0);
        current = gameObject.GetComponent<Tilemap>().tileAnchor;
        Debug.Log(current);
        StartCoroutine(createPoint());
    }

    
    void Update()
    {
        current = gameObject.GetComponent<Tilemap>().tileAnchor;
        //target = new Vector2(x, y);
        float step = speed * Time.deltaTime;
        gameObject.GetComponent<Tilemap>().tileAnchor = Vector2.MoveTowards(gameObject.GetComponent<Tilemap>().tileAnchor, target, step);

        if (current == target)
        {
            //claw down
            Debug.Log("I have hit my target!");
        }
    }

    IEnumerator createPoint()
    {
        while (true)
        {
            /*yield return new WaitForSeconds(3f);
            x++;
            y++;
            Debug.Log(target);*/

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

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            //claw down
            Debug.Log("I have touched the border!");
        }
    }*/

    void createRandomPoint()
    {
        target = new Vector2(Random.Range(minOffset, maxOffset), Random.Range(minOffset, maxOffset));
        Debug.Log("Target is random and is located at:" + target);
    }

    void createTargetedPoint()
    {
        //player.GetComponent<Tilemap>().tileAnchor.x, player.GetComponent<Tilemap>().tileAnchor.y (In case Natalia switches to using tile offset (which is doubt, but that's ok) :')
        target = new Vector2(player.GetComponent<Tilemap>().tileAnchor.x, player.GetComponent<Tilemap>().tileAnchor.y);
        Debug.Log("Target has been made using player position and it is located at: " + target);
    }

}
