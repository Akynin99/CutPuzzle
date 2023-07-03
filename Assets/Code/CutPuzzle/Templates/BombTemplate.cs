using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

public class BombTemplate : ViewComponent
{
    [Header("GD settings")]
    [Range(0f, 50f)] public float Range = 1f;
    [Range(0f, 10000f)] public float ExplosionForce = 100f;
    [Range(0f, 100f)] public float DetonateImpulse;
    public bool PushBall;
    
    [Header("Dev settings")]
    public ParticleSystem ExplosionFx;
    public GameObject Visuals;
    public Rigidbody Rigidbody;
    public Collider Collider;
    public LayerMask CollisionMask;
    public Transform ObjectForRope;
    public bool DestroyRopes;

    private EcsEntity _ecsEntity;
    private EcsWorld _ecsWorld;
    
    public override void EntityInit(EcsEntity ecsEntity, EcsWorld ecsWorld, bool parentOnScene)
    {
        _ecsEntity = ecsEntity;
        _ecsWorld = ecsWorld;
        
        ExplosionFx.transform.localScale *= Range;
        
        ecsEntity.Get<Bomb>() = new Bomb()
        {
           Range = Range,
           ExplosionForce = ExplosionForce,
           ExplosionFx = ExplosionFx,
           DetonateImpulse = DetonateImpulse,
           Visuals = Visuals,
           Rigidbody = Rigidbody,
           Collider = Collider,
           PushBall = PushBall,
           ObjectForRope = ObjectForRope,
           DestroyRopes = DestroyRopes,
        };
    }

    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log(other.impulse.magnitude + " " + other.gameObject);
        // Debug.Break();
        
        if(CollisionMask != (CollisionMask | (1 << other.gameObject.layer)))
            return;

        if (other.gameObject.layer == (int) LayerList.Human)
        {
            _ecsEntity.Get<BombDetonate>();
        }

        float impulse = other.impulse.magnitude;

        if (_ecsEntity.Has<BombCollision>())
            impulse += _ecsEntity.Get<BombCollision>().Impulse;
        
        _ecsEntity.Get<BombCollision>() = new BombCollision()
        {
            Impulse = impulse,
        };
    }

    private void OnDrawGizmosSelected()
    {
        Color color = Color.red;
        color.a = 0.2f;
        Gizmos.color = color;
        
        Gizmos.DrawSphere(transform.position, Range);
    }
}

public struct Bomb
{
    public float Range;
    public float ExplosionForce;
    public ParticleSystem ExplosionFx;
    public float DetonateImpulse;
    public GameObject Visuals;
    public Rigidbody Rigidbody;
    public Collider Collider;
    public bool PushBall;
    public Transform ObjectForRope;
    public bool DestroyRopes;
}

