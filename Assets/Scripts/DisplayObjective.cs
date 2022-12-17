using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayObjective : MonoBehaviour
{
    public GameObject objective;
    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = "Goal: Collect " + objective.GetComponent<GameObjective>().totalAnimals.ToString() + " stuffed animals!";
    }

}
