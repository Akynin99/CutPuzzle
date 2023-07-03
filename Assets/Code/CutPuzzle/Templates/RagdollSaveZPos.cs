using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class RagdollSaveZPos : MonoBehaviour
{
     private Rigidbody[] _rigidbodies;

     public void SetRigidBodies(Rigidbody[] rigidbodies)
     {
          _rigidbodies = rigidbodies;
     }

     private void FixedUpdate()
     {
          if(_rigidbodies == null)
               return;
          
          foreach (var rigidbody in _rigidbodies)
          {
               Vector3 velocity = rigidbody.velocity;
               velocity.z = 0;
               rigidbody.velocity = velocity;
          }
     }
}
