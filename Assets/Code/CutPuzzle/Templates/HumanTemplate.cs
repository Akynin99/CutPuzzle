using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class HumanTemplate : ViewElement
{
    public Collider Collider;
    public Rigidbody Rigidbody;
    public Animator Animator;
    public RagdollComponent RagdollComponent;
    public ParticleSystem DeathFx;
    public ParticleSystem BatutFx;
    public ParticleSystem FightFx;
    public Transform[] HitPoints;
    public RagdollSaveZPos SaveZPos;
    public Rigidbody[] BonesForAttraction;
    public Rigidbody BoneForPosition;
    public Renderer[] MainRenderers;
    public Material DeathMaterial;
    
    private static readonly int Emotion = Animator.StringToHash("emotion");

    public override void OnSpawn(EcsEntity entity, EcsWorld world)
    {
        base.OnSpawn(entity, world);

        entity.Get<Human>() = new Human()
        {
            Collider = Collider,
            Rigidbody = Rigidbody,
            HitPoints = HitPoints,
            Animator = Animator,
            DeathFx = DeathFx,
            BatutFx = BatutFx,
            FightFx = FightFx,
            BonesForAttraction = BonesForAttraction,
            BoneForPosition = BoneForPosition,
            MainRenderers = MainRenderers,
            DeathMaterial = DeathMaterial,
        };
        
        Animator.SetFloat(Emotion, 0.3f);

        entity.Get<RagdollComponent>() = RagdollComponent;
        
        if(SaveZPos)
            SaveZPos.SetRigidBodies(RagdollComponent.Rigidbodies);
    }


}
public struct Human
{
    public Collider Collider;
    public Rigidbody Rigidbody;
    public Animator Animator;
    public Transform[] HitPoints;
    public Rigidbody[] BonesForAttraction;
    public Rigidbody BoneForPosition;
    public ParticleSystem DeathFx;
    public ParticleSystem BatutFx;
    public ParticleSystem FightFx;
    public Renderer[] MainRenderers;
    public Material DeathMaterial;
}