using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator PlayerAnim;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            //newState = Closing;
           PlayerAnim.Play("PlayerAniTestAR_Walk");
        }
        if (Input.GetKeyUp("w"))
        {
            //newState = Closing;
            PlayerAnim.Play("PlayerAniTestAR_Idle");
        }
        if (Input.GetKey("a"))
        {
            //newState = Closing;
            PlayerAnim.Play("PlayerAniTestAL_Walk");
        }
        if (Input.GetKeyUp("a"))
        {
            //newState = Closing;
            PlayerAnim.Play("PlayerAniTestAL_Idle");
        }
        if (Input.GetKey("s"))
        {
            //newState = Closing;
            PlayerAnim.Play("PlayerAniTestTWL_Walk");
        }
        if (Input.GetKeyUp("s"))
        {
            //newState = Closing;
            PlayerAnim.Play("PlayerAniTestTWL_Idle");
        }
        if (Input.GetKey("d"))
        {
            //newState = Closing;
            PlayerAnim.Play("PlayerAniTestTWR_Walk");
        }
        if (Input.GetKeyUp("d"))
        {
            //newState = Closing;
            PlayerAnim.Play("PlayerAniTestTWR_Idle");
        }
    }
}
