using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class talkBubble : MonoBehaviour
{
    public byte talkBubbleChannel = 0;

    public float bubbleSize = 6;
    
    private void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, bubbleSize);
    }
}
