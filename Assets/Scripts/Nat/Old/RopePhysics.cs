using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePhysics : MonoBehaviour
{
    void OnJointBreak2D(Joint2D brokenJoint)
    {
        GameObject.Find("Player").GetComponent<PlayerControls>().getBrokenJoint(brokenJoint);
    }
}
