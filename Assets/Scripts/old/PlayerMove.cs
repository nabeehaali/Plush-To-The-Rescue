using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameObject grid;
    public GameObject animalPrefab;
    public float speed;
    int animalsCollected;

    Vector2 left, right, up, down;
    float mainTriangleArea;
    bool inBottomTriangle;
    bool inTopTriangle;
    public Vector2 randomPoint;

    void Start()
    {
        animalsCollected = 0;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        gameObject.transform.position = new Vector2(transform.position.x + (h * speed), transform.position.y + (v * speed));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Animal")
        {
            animalsCollected++;

            //in your case, it would be add it to the rope instead of destroy
            Destroy(collision.gameObject);

            if(animalsCollected >= 2)
            {
                StartCoroutine(createAnimal());
            }
        }
    }

    IEnumerator createAnimal()
    {
        int i = Random.Range(0, 10);
        yield return new WaitForSeconds(i);
        randomPoint = new Vector2(Random.Range(-1.95f, 2f), Random.Range(-3.57f, -5.27f));
        Instantiate(animalPrefab, randomPoint, Quaternion.identity);
        
    }
}
