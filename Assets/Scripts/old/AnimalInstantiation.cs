using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AnimalInstantiation : MonoBehaviour
{
    //Place this script on the grid gameobject which will hold the ground tilemap, also make sure that the tile for the animal is in the foreground! (maybe...)

    public GameObject animalPrefabCat;
    public GameObject animalPrefabRabbit;
    public float minOffset;
    public float maxOffset;
    public float numAnimalsStart;
    void Start()
    {
        
            for (int i = 0; i < numAnimalsStart; i++)
            {
                var SampleAnimal = Instantiate(animalPrefabCat, new Vector2(animalPrefabCat.transform.position.x, animalPrefabCat.transform.position.y), Quaternion.identity);
                animalPrefabCat.GetComponent<Tilemap>().tileAnchor = new Vector2(Random.Range(minOffset, maxOffset), Random.Range(minOffset, maxOffset));
                SampleAnimal.transform.parent = gameObject.transform;
            }
                
    }

}
