using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class finalScore : MonoBehaviour
{
    public TextMeshProUGUI catQty, whaleQty, cowQty, rabbitQty, chickenQty, total;
 
    void Start()
    {
        catQty.text = "" + GameObject.Find("PointManager").GetComponent<PointManager>().catCount;
        whaleQty.text = "" + GameObject.Find("PointManager").GetComponent<PointManager>().whaleCount;
        cowQty.text = "" + GameObject.Find("PointManager").GetComponent<PointManager>().cowCount;
        chickenQty.text = "" + GameObject.Find("PointManager").GetComponent<PointManager>().chickenCount;
        rabbitQty.text = "" + GameObject.Find("PointManager").GetComponent<PointManager>().rabbitCount;

        total.text = "" + (GameObject.Find("PointManager").GetComponent<PointManager>().catCount + GameObject.Find("PointManager").GetComponent<PointManager>().whaleCount + GameObject.Find("PointManager").GetComponent<PointManager>().cowCount + GameObject.Find("PointManager").GetComponent<PointManager>().chickenCount + GameObject.Find("PointManager").GetComponent<PointManager>().rabbitCount);
    }
}
