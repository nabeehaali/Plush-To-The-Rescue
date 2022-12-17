using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class SoundManager: MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;

    // public AudioClip ClawClose;

    public AudioClip[] AllAudioClips;
    public bool GameEnded; 
    void Start()
    {
        audioSource.clip = AllAudioClips[2];
        audioSource.Play();
        GameEnded = false; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlaySounds(string AudioName) {
        if (GameEnded == true) { 
        //do nothing
        }
        else
        {
            switch (AudioName)
            {
                case "ClawClose":
                    audioSource.PlayOneShot(AllAudioClips[0]);
                   
                    break;
                case "ClawUp":
                    audioSource.PlayOneShot(AllAudioClips[1]);

                    break;
                case "ClawDown":
                    audioSource.PlayOneShot(AllAudioClips[2]);
  
                    break;
                case "Collection":
                    audioSource.PlayOneShot(AllAudioClips[3]);
                    break;
                case "LoseStuffy":
                    audioSource.PlayOneShot(AllAudioClips[4]);
                    break;
                case "CaughtStuffy":
                    audioSource.PlayOneShot(AllAudioClips[5]);
                    break;
                case "Footprint1":
                    audioSource.PlayOneShot(AllAudioClips[6]);
                    break;
                case "Footprint2":
                    audioSource.PlayOneShot(AllAudioClips[7]);
                    break;
                case "ThrowStuffySound":
                    audioSource.PlayOneShot(AllAudioClips[7]);
                    break;
                case "ClawSide":
                    audioSource.PlayOneShot(AllAudioClips[10]);
                    break;
                case "ClawClick1":
                    audioSource.PlayOneShot(AllAudioClips[11]);
                  
                    break;
             
                case "bump":
                  
                    audioSource.PlayOneShot(AllAudioClips[12]);
           
                    break;

                case "ThrowStuffyPostive":
                    audioSource.PlayOneShot(AllAudioClips[13]);
                    break;
                case "CompleteMission":
                    audioSource.PlayOneShot(AllAudioClips[14]);
                    break;
                case "NewStuffy":
                    audioSource.PlayOneShot(AllAudioClips[15]);
                    break;
                case "NewObstacle":
                    audioSource.PlayOneShot(AllAudioClips[16]);
                    break;
                case "LoseSong":
                    
                    audioSource.clip = AllAudioClips[17];
                   
                    break;
            }
        }
        
        

    }
    //this stops the issue of lots of sound near the end of the game
    public void GameHasEnded() {
        GameEnded = true; 
    }

    //Wanted to make another function to test out something but it didn't work, its pretty much the same function 
    //as the normal play sounds function except GameHasEnded has no effect so claw moving around sounds still play 
    //int Action has no meaning
    public void ClawSounds(string ClawAudioName, int Action)
    {
       
        switch (ClawAudioName)
        {
            case "ClawDown":
                audioSource.clip = AllAudioClips[2];
              
                audioSource.Play();
            
                break;
            case "ClawUp":
                audioSource.clip = AllAudioClips[1];
                audioSource.Play();
             
                break;
            case "ClawSide":
                audioSource.clip = AllAudioClips[10];
                if (!audioSource.isPlaying) {
                    audioSource.Play();
                }
                break;
        }
    }

    //this was for testing purposes
    public void NoLongClawSounds() {
        audioSource.clip = null; 
    }
   
}
