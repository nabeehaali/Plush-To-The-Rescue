using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigate : MonoBehaviour
{
    public AudioSource ButtonSound; 
    public void LoadScene(string sceneName)
    {
        Destroy(GameObject.Find("PointManager").gameObject);
        ButtonSound.Play();
        SceneManager.LoadScene(sceneName);
        
    }
}
