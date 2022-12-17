using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointManager : MonoBehaviour
{
    // Plushy points
    public int catCount = 0;
    public int rabbitCount = 0;
    public int chickenCount = 0;
    public int cowCount = 0;
    public int whaleCount = 0;

    // Total points
    public int totalAnimals = 0;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void resetScore()
    {
        totalAnimals = 0;

        catCount = 0;
        rabbitCount = 0;
        chickenCount = 0;
        cowCount = 0;
        whaleCount = 0;
    }

    void Update()
    {
        totalAnimals = catCount + cowCount + chickenCount + rabbitCount + whaleCount;
    }
}
