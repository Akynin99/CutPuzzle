using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
     [Header("GD Settings")]
     [Range(-1f, 1f)] public float AxisPosition;
     [Range(0f, 10f)] public float PlatformLength;
     
     [Header("Dev Settings")]
     public Transform Axis;
     public Transform Platform;
     public HingeJoint HingeJoint;
     
     private void OnDrawGizmos()
     {
          
          if(Application.isPlaying)
               return;
          
          if(!HingeJoint || !Platform)
               return;
          
          float pos = Mathf.Clamp(AxisPosition, -1, 1);
          
          Vector3 platformScale = Platform.localScale;
          platformScale.x = PlatformLength;
          Platform.localScale = platformScale;
          
          Vector3 anchor = HingeJoint.connectedAnchor;
          anchor.x = pos / 2f;
          HingeJoint.connectedAnchor = -anchor;
          Platform.localPosition = anchor * PlatformLength;
          
          
     }

}
