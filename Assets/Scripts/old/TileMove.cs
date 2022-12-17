using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMove : MonoBehaviour
{
    public float speed;

    void Start()
    {
        
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        gameObject.GetComponent<Tilemap>().tileAnchor = new Vector2(gameObject.GetComponent<Tilemap>().tileAnchor.x + (h * speed), gameObject.GetComponent<Tilemap>().tileAnchor.y + (v * speed));
    }
}
