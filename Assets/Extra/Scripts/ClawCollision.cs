using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawCollision : MonoBehaviour

{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        anim.Play("left_claw_grab");
        anim.Play("right_claw_grab");
    }
}
