using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerAnimatorEvents : MonoBehaviour
{
    public Animator animator;
    public string FloatPropertyName;
    public UnityEvent OnTriggerEnterEvent, OnTriggerExitEvent;
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent.Invoke();
    }
    public void SetNamedFloatValue(int value)
    {
        animator.SetInteger(FloatPropertyName, value);
    }
    public void SetAnimationBoolTrue(string Name) {
        animator.SetBool(Name, true);
    }
    public void SetAnimationBoolFalse(string Name)
    {
        animator.SetBool(Name, false);
    }
}
