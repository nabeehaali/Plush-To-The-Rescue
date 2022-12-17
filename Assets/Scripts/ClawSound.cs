using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawSound : MonoBehaviour
{
    // Start is called before the first frame update
    SoundManager SoundManager;

    void Start()
    {
        SoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClawClose() {

       
        SoundManager.PlaySounds("ClawClose");
        //SoundManager.PlaySounds("ClawCatchStuffy");
        SoundManager.PlaySounds("ClawClick1");
    }
}
