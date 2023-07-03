using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.Utils;
using Modules.ViewHub;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyTemplate : ViewElement
{
    [Header("GD settings")]
    public float PlatformKillImpulse;
    [Header("Dev settings")]
    public Renderer Renderer;
    public Collider Collider;
    public Rigidbody Rigidbody;
    public Animator Animator;
    public RagdollComponent RagdollComponent;
    public ParticleSystem FightFx;
    public Transform[] HitPoints;
    public Transform BoneForPosition;
    public GameObject[] AliveEmotes;
    public GameObject[] DeadEmotes;
    public EnemyVisualSwitcher VisualSwitcher;
    public Renderer[] MainRenderers;
    public Material DeathMaterial;
    
    private static readonly int Emotion = Animator.StringToHash("emotion");

    public override void OnSpawn(EcsEntity entity, EcsWorld world)
    {
        base.OnSpawn(entity, world);
        
        RagdollComponent.SetMode(false);

        entity.Get<Enemy>() = new Enemy()
        {
            Collider = Collider,
            HitPoints = HitPoints,
            Animator = Animator,
            AliveEmotes = AliveEmotes,
            DeadEmotes = DeadEmotes,
            PlatformKillImpulse = PlatformKillImpulse,
            BoneForPosition = BoneForPosition,
            FightFx = FightFx,
            MainRenderers = MainRenderers,
            DeathMaterial = DeathMaterial,
        };
        
        Animator.SetFloat(Emotion, 0.6f);

        entity.Get<RagdollComponent>() = RagdollComponent;
        
        VisualSwitcher.SetVisual(Random.Range(0, VisualSwitcher.EnemyVisuals.Length));
    }


}
public struct Enemy
{
    public Collider Collider;
    public Transform[] HitPoints;
    public Animator Animator;
    public GameObject[] AliveEmotes;
    public GameObject[] DeadEmotes;
    public float PlatformKillImpulse;
    public ParticleSystem FightFx;
    public Transform BoneForPosition;
    public Renderer[] MainRenderers;
    public Material DeathMaterial;
}