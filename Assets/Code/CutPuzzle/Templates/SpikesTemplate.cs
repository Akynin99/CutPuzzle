using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class SpikesTemplate : ViewComponent
{
    public LayerMask CollisionMask;
    public GameObject Visuals;
    public Collider Collider;
    public bool Destructable;
    
    public override void EntityInit(EcsEntity ecsEntity, EcsWorld ecsWorld, bool parentOnScene)
    {
        ecsEntity.Get<Spikes>() = new Spikes()
        {
            Visuals = Visuals,
            Collider = Collider,
            Destructable = Destructable,
        };
    }

    private void OnCollisionEnter(Collision other)
    {
        if(CollisionMask != (CollisionMask | (1 << other.gameObject.layer)))
            return;

        EntityRef entityRef = null;

        entityRef = other.transform.GetComponentInParent<EntityRef>();

        if (entityRef != null)
        {
            entityRef.Entity.Get<SpikeCollision>();
        }
    }

    
}
public struct Spikes
{
    public GameObject Visuals;
    public Collider Collider;
    public bool Destructable;
}