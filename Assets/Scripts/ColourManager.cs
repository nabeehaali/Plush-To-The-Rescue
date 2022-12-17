using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourManager : MonoBehaviour
{
    public GameObject[] clawParts;
    public Color[] newColour;
    public float time;
    public AudioSource loseSound;

    //change colour of the environment when the claw grabs the player
    public void changeCol()
    {
        if (loseSound.isPlaying== false) {
            loseSound.Play();
        }
      
        for (int i = 0; i < clawParts.Length; i++)
        {
            clawParts[i].GetComponent<SpriteRenderer>().color = Color.Lerp(clawParts[i].GetComponent<SpriteRenderer>().color, newColour[i], time);
           
        }
      
    }
}
