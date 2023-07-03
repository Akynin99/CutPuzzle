using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class RotatingPlatformCollider : MonoBehaviour
{
    [Header("Dev settings")]
    
    public LayerMask CollisionMask;
     
    private void OnCollisionEnter(Collision other)
    {
        if(CollisionMask != (CollisionMask | (1 << other.gameObject.layer)))
            return;

        EntityRef entityRef = null;

        entityRef = other.transform.GetComponentInParent<EntityRef>();
        
        float impulse = other.impulse.magnitude;

        if (entityRef != null)
        {
            if(entityRef.Entity.Has<PlatformCollision>())
                impulse += entityRef.Entity.Get<PlatformCollision>().Impulse;
            
            entityRef.Entity.Get<PlatformCollision>().Impulse = impulse;
        }
    }

}
