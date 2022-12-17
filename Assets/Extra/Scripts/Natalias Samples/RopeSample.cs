using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSample : MonoBehaviour
{
    void OnJointBreak2D(Joint2D brokenJoint)
    {
        GameObject.Find("Player").GetComponent<PlayerSample>().getBrokenJoint(brokenJoint);
    }
}
