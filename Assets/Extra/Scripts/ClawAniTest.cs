using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAniTest : MonoBehaviour
{

    public Animator ClawAnim;
    public enum ClawState { Open, Closing, Closed}
    ClawState currentState;
    ClawState newState; 
    // Start is called before the first frame update
    void Start()
     
    {
       // ClawAnim.Play("ClawAniTest1",0,1/ (float)12);
        //currentState = Open;
        //newState = Open; 
    }

    // Update is called once per frame
    void Update()
    {
        //ClawAnim.Play("ClawAniTest1", 0, 7/ (float)12);

        if (Input.GetKeyDown("space"))
        {
            //newState = Closing;
            ClawAnim.Play("ClawAniTest1");
        }
    }
}
