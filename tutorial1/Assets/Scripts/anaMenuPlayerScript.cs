using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anaMenuPlayerScript : MonoBehaviour
{
    public Animator animator;
    float animationActive = 0;
    void Start()
    {
        
    }
    void Update()
    {
        animationActive += Time.deltaTime;
       if(animationActive>=6f)
        {
            animator.SetBool("secondAnimation", true);
        }
       if (animationActive >= 12f)
        {
            animator.SetBool("thirthAnimation", true);
        }
        if (animationActive >= 16f)
        {
            animator.SetBool("firstAnimation", true);
            animationActive = 0f;
        }
    }
}
