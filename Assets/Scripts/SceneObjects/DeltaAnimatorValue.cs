using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeltaAnimatorValue : MonoBehaviour
{
    public Animator animator;
    public float AnimateDelta;
    Vector3 previospos;
    void Start()
    {
        previospos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(previospos,transform.position)> AnimateDelta)
        {
            animator.SetBool("Walking", true);
        }else
            animator.SetBool("Walking", false);
        previospos = transform.position;
    }
}
