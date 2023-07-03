using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class DeathToHuman : ViewComponent
{
    
    
    public override void EntityInit(EcsEntity ecsEntity, EcsWorld ecsWorld, bool parentOnScene)
    {
        // ecsEntity.Get<Hole>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer != 3)
            return;

        EntityRef entityRef = null;

        entityRef = other.transform.GetComponentInParent<EntityRef>();

        if (entityRef != null && entityRef.Entity.Has<Human>())
        {
            entityRef.Entity.Get<DeathTag>();
        }
    }

    
}
