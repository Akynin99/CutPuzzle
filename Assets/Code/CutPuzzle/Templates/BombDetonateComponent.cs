using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class BombDetonateComponent : ViewComponent
{
    public LayerMask CollisionMask;
    
    public override void EntityInit(EcsEntity ecsEntity, EcsWorld ecsWorld, bool parentOnScene)
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(CollisionMask != (CollisionMask | (1 << other.gameObject.layer)))
            return;

        EntityRef entityRef = null;

        entityRef = other.transform.GetComponentInParent<EntityRef>();

        if (entityRef != null)
        {
            entityRef.Entity.Get<BombDetonate>();
        }
    }

    
}
