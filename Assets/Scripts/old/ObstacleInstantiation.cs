using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleInstantiation : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float minOffset = 0.5f;
    public float maxOffset = 7.5f;
    public float numObstcles;
    void Start()
    {
        for (int i = 0; i < numObstcles; i++)
        {
            var obstacle = Instantiate(obstaclePrefab, new Vector2(obstaclePrefab.transform.position.x, obstaclePrefab.transform.position.y), Quaternion.identity);
            obstaclePrefab.GetComponent<Tilemap>().tileAnchor = new Vector2(Random.Range(minOffset, maxOffset), Random.Range(minOffset, maxOffset));
            obstacle.transform.parent = gameObject.transform;
        }
    }
}
