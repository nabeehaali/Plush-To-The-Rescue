using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        GameObject[] music = GameObject.FindGameObjectsWithTag("GameMusic");
        if (music.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);

    }

}
