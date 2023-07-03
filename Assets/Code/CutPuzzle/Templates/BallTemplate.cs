using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

public class BallTemplate : ViewComponent
{
    
    
    [Header("Dev settings")]
    public GameObject Visuals;
    public Rigidbody Rigidbody;
    public LayerMask CollisionMask;
    public bool CanKillPlayer;

    private EcsEntity _ecsEntity;
    private EcsWorld _ecsWorld;
    
    public override void EntityInit(EcsEntity ecsEntity, EcsWorld ecsWorld, bool parentOnScene)
    {
        _ecsEntity = ecsEntity;
        _ecsWorld = ecsWorld;

        ecsEntity.Get<Ball>() = new Ball()
        {
           Visuals = Visuals,
           Rigidbody = Rigidbody,
           CanKillPlayer = CanKillPlayer,
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
            entityRef.Entity.Get<BallCollision>().CanKillPlayer = CanKillPlayer;
        }
    }

}

public struct Ball
{
    
    public GameObject Visuals;
    public Rigidbody Rigidbody;
    public bool CanKillPlayer;
}

