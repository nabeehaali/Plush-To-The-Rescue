using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameObjective : MonoBehaviour
{
    [SerializeField]
    public int minAnimals;
    public int maxAnimals;

    public int totalAnimals;
    //use totalAnimals variable to determine whether num stuffies on conga == totalAnimals

    void Start()
    {
        totalAnimals = Random.Range(minAnimals, maxAnimals);
    }

    public void generateNumAnimals()
    {
        totalAnimals = Random.Range(minAnimals, maxAnimals);
        gameObject.GetComponent<TextMeshProUGUI>().fontSize = 79.6f;
        if(totalAnimals > 1)
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = "rescue " + totalAnimals.ToString() + "\n plushies!";
        }
        else
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = "rescue " + totalAnimals.ToString() + "\n plushie!";
        }

    }

    public void changeMessage()
    {
        gameObject.GetComponent<TextMeshProUGUI>().fontSize = 64;
        gameObject.GetComponent<TextMeshProUGUI>().text = "throw " + totalAnimals.ToString() + " plushies in the prize chute!";
    }

    public void caughtPlayer()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(8.045f, 710);
        gameObject.GetComponent<TextMeshProUGUI>().text = "you have been caught by claw, you lose!";
        GameObject.Find("ThrowAmount").GetComponent<TextMeshProUGUI>().enabled = false;
    }

    public int getAmount()
    {
        return totalAnimals;
    }

}
