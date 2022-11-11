using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyPlayer : MonoBehaviour
{
    public PlayerState SwitchPlayerTo;
    public string jsonSettingsToApply;
}
public enum PlayerState 
{
    None,
    SmoothLocomotion,
    FlyCam
}