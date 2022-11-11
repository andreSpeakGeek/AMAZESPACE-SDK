using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportStation : MonoBehaviour
{
    public teleportStation destination;

    public void TelePortToThisTeleportStation() 
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, destination.transform.position);
    }
}
