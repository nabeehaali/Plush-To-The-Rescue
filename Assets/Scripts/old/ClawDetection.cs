using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawDetection : MonoBehaviour
{
    public bool isActive;
    public Transform parent;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isActive = true;
        if(collision.gameObject.tag == "Animal" || collision.gameObject.tag == "Collected" || collision.gameObject.tag == "Obstacle")
        {
            collision.gameObject.transform.SetParent(parent);
            Debug.Log("The stuffy is captured by the claw");
        }
        
    }

}
