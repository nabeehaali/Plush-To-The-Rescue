using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBearAnimation : MonoBehaviour
{
    public Animator PlayerAnim;
    SoundManager SoundManager;
    public Vector2 joystickMovement;
    public Vector2 Vector22dot5Deg;
    Vector2 Vector0; 

    enum LastKey { N, W, E, S, NE, NW, SE,SW};
    public float AnimationSpeed; 
    LastKey CurrentLastKey;
  

    // Start is called before the first frame update

  

    void Start()
    {
        SoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        CurrentLastKey = LastKey.SE;
        AnimationSpeed = 1.0f;
       
        Vector0.x = 0.0f;
        Vector0.y = 0.0f;
         SoundManager.PlaySounds("ClawClose");

       
    }


    // Update is called once per frame
    void Update()
    {

    
        joystickMovement.x = Input.GetAxisRaw("JS1_hor");
        joystickMovement.y = Input.GetAxisRaw("JS1_ver");

        int numOfKeys = CountNumofKeys();
        if (numOfKeys == 1)
        {
            
            if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
            {
CurrentLastKey = LastKey.N;
                PlayerAnim.Play("BearN_Walk");
            }
            if (Input.GetKeyUp("w") || Input.GetKeyUp(KeyCode.UpArrow))
            {
CurrentLastKey = LastKey.N;
                PlayerAnim.Play("BearN_Idle");
            }
            if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
            {
CurrentLastKey = LastKey.W;
                PlayerAnim.Play("BearW_Walk");
            }
            if (Input.GetKeyUp("a") || Input.GetKeyUp(KeyCode.LeftArrow))
            {
CurrentLastKey = LastKey.W;

                PlayerAnim.Play("BearW_Idle");
            }
            if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))
            {
CurrentLastKey = LastKey.S;
                PlayerAnim.Play("BearS_Walk");
            }
            if (Input.GetKeyUp("s") || Input.GetKeyUp(KeyCode.DownArrow))
            {
CurrentLastKey = LastKey.S;
                PlayerAnim.Play("BearS_Idle");
            }
            if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
            {
CurrentLastKey = LastKey.E;
                PlayerAnim.Play("BearE_Walk");
            }
            if (Input.GetKeyUp("d") || Input.GetKeyUp(KeyCode.RightArrow))
            {
CurrentLastKey = LastKey.E;
                PlayerAnim.Play("BearE_Idle");
            }
        }
        else {
            if ((Input.GetKey("w") && Input.GetKey("a")) || (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow)) )
            {
CurrentLastKey = LastKey.NW;
                PlayerAnim.Play("BearNW_Walk");
            }
            if (Input.GetKey("w") && Input.GetKey("d") || (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow)))
            {
CurrentLastKey = LastKey.NE;
                PlayerAnim.Play("BearNE_Walk");
            }
            if (Input.GetKey("s") && Input.GetKey("a") || (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow)))
            {
CurrentLastKey = LastKey.SW;
                PlayerAnim.Play("BearSW_Walk") ;
            }
            if (Input.GetKey("s") && Input.GetKey("d") || (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow)) )
            {
CurrentLastKey = LastKey.SE;
                PlayerAnim.Play("BearSE_Walk");
            }


            if ( (Input.GetKeyUp("w") && Input.GetKeyUp("a")) || (Input.GetKeyUp(KeyCode.UpArrow) && Input.GetKeyUp(KeyCode.LeftArrow)))
            {
CurrentLastKey = LastKey.NW;
                PlayerAnim.Play("BearNW_Idle");
            }
            if (Input.GetKeyUp("w") && Input.GetKeyUp("d") || (Input.GetKeyUp(KeyCode.UpArrow) && Input.GetKeyUp(KeyCode.RightArrow)) )
            {
CurrentLastKey = LastKey.NE;
                PlayerAnim.Play("BearNE_Idle");
            }
            if (Input.GetKeyUp("s") && Input.GetKeyUp("a") || (Input.GetKeyUp(KeyCode.DownArrow) && Input.GetKeyUp(KeyCode.LeftArrow)) )
            {
CurrentLastKey = LastKey.SW;
                PlayerAnim.Play("BearSW_Idle");
            }
            if (Input.GetKeyUp("s") && Input.GetKeyUp("d") || (Input.GetKeyUp(KeyCode.DownArrow) && Input.GetKeyUp(KeyCode.RightArrow)))
            {
CurrentLastKey = LastKey.SE;
                PlayerAnim.Play("BearSE_Idle");
            }
        }
        if (numOfKeys == 0 && joystickMovement == Vector0) {
            // N, W, E, S, NE, NW, SE,SW
            switch (CurrentLastKey) {
                case LastKey.N:
                    PlayerAnim.Play("BearN_Idle");
                    break;
                case LastKey.W:
                    PlayerAnim.Play("BearW_Idle");
                    break;
                case LastKey.E:
                    PlayerAnim.Play("BearE_Idle");
                    break;
                case LastKey.S:
                    PlayerAnim.Play("BearS_Idle");
                    break;
                case LastKey.NE:
                    PlayerAnim.Play("BearNE_Idle");
                    break;
                case LastKey.NW:
                    PlayerAnim.Play("BearNW_Idle");
                    break;
                case LastKey.SE:
                    PlayerAnim.Play("BearSE_Idle");
                    break;
                case LastKey.SW:
                    PlayerAnim.Play("BearSW_Idle");
                    break;
                default:
                    PlayerAnim.Play("BearSE_Idle");
                    break;
            }
        
        
        
        }
        else if (numOfKeys == 0)
        {
            ControllerAnimation();
        }

    }

    int CountNumofKeys()
    {
        int NumKeys = 0;
        if (Input.GetKey("w")) {
         
            NumKeys++;
           
        }
        if (Input.GetKey("a"))
        {
         
            NumKeys++;
        }
        if (Input.GetKey("s"))
        {
          
            NumKeys++;
        }
        if (Input.GetKey("d"))
        {
            NumKeys++;
          
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
           
            NumKeys++;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            NumKeys++;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            NumKeys++;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            NumKeys++;
        }
   
        return NumKeys;
    }

    public void footprintSound1()
    {

        SoundManager.PlaySounds("Footprint1");
      
    }

    public void footprintSound2()
    {

        SoundManager.PlaySounds("Footprint2");

    }

    public void PlayerAnimationSpeed(int ListSize) {
        //min is 0.2
        //max is 1 
        //you lose 0.1 based on every animal in the list 
        // PlayerControlsWithSoundKatie.congaLine;
        
        if (ListSize == 0) {
            PlayerAnim.speed = 1; 
        }
        if (ListSize == 1)
        {
            PlayerAnim.speed = 0.8f;
        }
        if (ListSize == 2)
        {
            PlayerAnim.speed = 0.6f;
        }
        if (ListSize == 3)
        {
            PlayerAnim.speed = 0.4f;
        }
        if (ListSize >= 4)
        {
            PlayerAnim.speed = 0.2f;
        }
        // PlayerAnim.speed = AnimationSpeed;
    }

    public void ControllerAnimation()
    {
        //this vector makes an angle with the movement vector
        //that I use to compare and calculate the animation of the bear 
        //this vector makes a 22.5 deg angles compared to the horizontal
        //this is because there are 8 different angles 
        // 1/8th of 360 is 45% and half of that is 22.5% 
        // i did this so that more intuitive angles 0 and 45 would be correct 
        Vector22dot5Deg.x = 0.5f;
        Vector22dot5Deg.y = 0.20710f;


        // Debug.Log(joystickMovement);
        float angle = Vector2.SignedAngle(Vector22dot5Deg, joystickMovement);


        switch (angle)
        {
            case float i when i > 0 && i <= 45:
                PlayerAnim.Play("BearNE_Walk");
                CurrentLastKey = LastKey.NE;
                break;
            case float i when i > 45 && i <= 90:
                PlayerAnim.Play("BearN_Walk");
                CurrentLastKey = LastKey.N;
                break;
            case float i when i > 90 && i <= 135:
                PlayerAnim.Play("BearNW_Walk");
                CurrentLastKey = LastKey.NW;
                break;
            case float i when i > 135 && i <= 180:
                PlayerAnim.Play("BearW_Walk");
                CurrentLastKey = LastKey.W;
                break;
            case float i when i <= 0 && i >= -45:
                PlayerAnim.Play("BearE_Walk");
                CurrentLastKey = LastKey.E;
                break;
            case float i when i < -45 && i >= -90:
                PlayerAnim.Play("BearSE_Walk");
                CurrentLastKey = LastKey.SE;
                break;
            case float i when i < -90 && i >= -135:
                PlayerAnim.Play("BearS_Walk");
                CurrentLastKey = LastKey.S;
                break;
            case float i when i < -135 && i >= -180:
                PlayerAnim.Play("BearSW_Walk");
                CurrentLastKey = LastKey.SW;
                break;

            default:
                PlayerAnim.Play("BearSE_Walk");
                break;

        }
    }
}
