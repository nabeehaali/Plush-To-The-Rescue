using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BinDetector : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "PrizeBin" &&  SceneManager.GetActiveScene().name == "Tutorial")
        {
            GameObject.Find("Player").GetComponent<PlayerTutorial>().binDistance = true;
        }
        else if (col.gameObject.tag == "PrizeBin" && SceneManager.GetActiveScene().name == "MainGame")
        {
            GameObject.Find("Player").GetComponent<MainPlayerScript>().binDistance = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "PrizeBin" && SceneManager.GetActiveScene().name == "Tutorial")
        {
            GameObject.Find("Player").GetComponent<PlayerTutorial>().binDistance = false;
        }
        else if (col.gameObject.tag == "PrizeBin" && SceneManager.GetActiveScene().name == "MainGame")
        {
            GameObject.Find("Player").GetComponent<MainPlayerScript>().binDistance = false;
        }
    }
}
