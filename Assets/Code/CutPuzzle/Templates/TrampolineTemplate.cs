using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

public class TrampolineTemplate : ViewComponent
{
    [Header("GD Settings")]
    [Range(0f, 40f)]public float DragDistance;
    [Range(0f, 180f)]public float AngleDirection;
    [Range(-180f, 180f)]public float PushDirection;
    [Range(0f, 500f)]public float PushForce;
    [Range(-1f, 1f)]public float StartPos;
    public bool UseSpecialPushForces;
    [Range(0f, 500f)]public float SpecialBallPushForce;
    [Range(0f, 500f)]public float SpecialBombPushForce;
    [Range(0f, 500f)]public float SpecialHumanPushForce;
    
    [Header("Dev Settings")]
    public Transform Root;
    public Transform Path;
    public Animator Animator;
    public ParticleSystem JumpFx;
    public LayerMask CollisionMask;
    
    
    private EcsWorld _world;
    private EcsEntity _ecsEntity;
    
    public override void EntityInit(EcsEntity ecsEntity, EcsWorld ecsWorld, bool parentOnScene)
    {
        _world = ecsWorld;
        _ecsEntity = ecsEntity;
        Vector3 dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * AngleDirection), Mathf.Sin(Mathf.Deg2Rad * AngleDirection),0);

        Path.right = dir;
        Path.localScale = new Vector3(DragDistance*2, 1, 1);
        Path.parent = transform.parent;
        
        ecsEntity.Get<Trampoline>() = new Trampoline()
        {
            MoveEnd1 = Root.position + dir * DragDistance,
            MoveEnd2 = Root.position - dir * DragDistance,
            PushForce = PushForce,
            Animator = Animator,
            UseSpecialPushForces = UseSpecialPushForces,
            SpecialBallPushForce = SpecialBallPushForce,
            SpecialBombPushForce = SpecialBombPushForce,
            SpecialHumanPushForce = SpecialHumanPushForce,
            JumpFx = JumpFx,
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
            _world.NewEntity().Get<TrampolineCollision>() = new TrampolineCollision()
            {
                TrampolineEntity = _ecsEntity,
                OtherEntity = entityRef.Entity,
            };
        }
        else if (other.rigidbody != null)
        {
            other.rigidbody.AddForce(PushForce * transform.up, ForceMode.Impulse);
            Animator.Play("tram_1_action_1");
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * AngleDirection), Mathf.Sin(Mathf.Deg2Rad * AngleDirection),0);

        Vector3 pos = Root.position;

        Gizmos.DrawLine(pos, pos + dir * DragDistance);
        Gizmos.DrawLine(pos, pos - dir * DragDistance);
        
        Gizmos.color = Color.blue;

        Vector3 endArrowPoint = transform.position + transform.up * 2;
        Vector3 rightArrowWing = endArrowPoint - (transform.up + transform.right) * 0.25f;
        Vector3 leftArrowWing = endArrowPoint - (transform.up - transform.right) * 0.25f;
        Gizmos.DrawLine(transform.position, endArrowPoint);
        Gizmos.DrawLine(endArrowPoint, rightArrowWing);
        Gizmos.DrawLine(endArrowPoint, leftArrowWing);
        
        if(Application.isPlaying)
            return;
        
        transform.localPosition = dir * DragDistance * StartPos;
        Vector3 pushDir = new Vector3(0, 0, PushDirection);
        transform.localRotation = Quaternion.Euler(pushDir);
        transform.parent.localRotation = Quaternion.identity;
        
        
    }
}

public struct Trampoline
{
    public Vector3 MoveEnd1;
    public Vector3 MoveEnd2;
    public float PushForce;
    public Animator Animator;
    public ParticleSystem JumpFx;
    public float DistanceTraveled;
    public bool UseSpecialPushForces;
    public float SpecialBallPushForce;
    public float SpecialBombPushForce;
    public float SpecialHumanPushForce;
}
