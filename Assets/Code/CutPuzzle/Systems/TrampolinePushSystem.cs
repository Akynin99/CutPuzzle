using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class TrampolinePushSystem : IEcsRunSystem
{
    
    private EcsFilter<TrampolineCollision> _filter;

    private EcsWorld _world;

    public void Run()
    {
        foreach (var i in _filter)
        {
            EcsEntity otherEntity = _filter.Get1(i).OtherEntity;
            EcsEntity trampEntity = _filter.Get1(i).TrampolineEntity;
            if (otherEntity.Has<Human>() || otherEntity.Has<Enemy>())
            {
                bool timeOut = false;

                if (otherEntity.Has<TrampolineTimeOut>())
                {
                    if (otherEntity.Get<TrampolineTimeOut>().TrampolineEntity.Equals(trampEntity))
                    {
                        timeOut = true;
                    }
                }
                
                if(!timeOut)
                    PushHuman(_filter.Get1(i).TrampolineEntity, otherEntity);
            }
            
            if (otherEntity.Has<Bomb>())
            {
                bool timeOut = false;

                if (otherEntity.Has<TrampolineTimeOut>())
                {
                    if (otherEntity.Get<TrampolineTimeOut>().TrampolineEntity.Equals(trampEntity))
                    {
                        timeOut = true;
                    }
                }
                
                if(!timeOut)
                    PushBomb(_filter.Get1(i).TrampolineEntity, otherEntity);
            }
            
            if (otherEntity.Has<Ball>())
            {
                bool timeOut = false;

                if (otherEntity.Has<TrampolineTimeOut>())
                {
                    if (otherEntity.Get<TrampolineTimeOut>().TrampolineEntity.Equals(trampEntity))
                    {
                        timeOut = true;
                    }
                }
                
                if(!timeOut)
                    PushBall(_filter.Get1(i).TrampolineEntity, otherEntity);
            }

            _filter.GetEntity(i).Del<TrampolineCollision>();
        }
    }


    private void PushHuman(EcsEntity trampolineEntity, EcsEntity otherEntity)
    {
        ref var tramp = ref trampolineEntity.Get<Trampoline>();
        Transform trampTransform = trampolineEntity.Get<UnityView>().Transform;

        RagdollComponent ragdoll = otherEntity.Get<RagdollComponent>();
        ParticleSystem vfx =  otherEntity.Get<Human>().BatutFx;
        
        if(vfx)
            vfx.Play();

        float force = tramp.UseSpecialPushForces ? tramp.SpecialHumanPushForce : tramp.PushForce;

        foreach (var rigidbody in ragdoll.Rigidbodies)
        {
            rigidbody.AddForce(force * trampTransform.up, ForceMode.Impulse);
        }
                
        otherEntity.Get<TrampolineTimeOut>() = new TrampolineTimeOut()
        {
            Time = 0.3f,
            TrampolineEntity = trampolineEntity,
        };
        
        JumpAnimation(tramp.Animator);
        tramp.JumpFx.Play();
    }
    
    private void PushBomb(EcsEntity trampolineEntity, EcsEntity otherEntity)
    {
        ref var tramp = ref trampolineEntity.Get<Trampoline>();
        Transform trampTransform = trampolineEntity.Get<UnityView>().Transform;

        ref var bomb = ref otherEntity.Get<Bomb>();
        
        float force = tramp.UseSpecialPushForces ? tramp.SpecialBombPushForce : tramp.PushForce;

        bomb.Rigidbody.AddForce(force * trampTransform.up, ForceMode.Impulse);
                
        otherEntity.Get<TrampolineTimeOut>() = new TrampolineTimeOut()
        {
            Time = 0.3f,
            TrampolineEntity = trampolineEntity,
        };
        
        JumpAnimation(tramp.Animator);
        tramp.JumpFx.Play();
    }
    
    private void PushBall(EcsEntity trampolineEntity, EcsEntity otherEntity)
    {
        ref var tramp = ref trampolineEntity.Get<Trampoline>();
        Transform trampTransform = trampolineEntity.Get<UnityView>().Transform;

        ref var ball = ref otherEntity.Get<Ball>();
        
        float force = tramp.UseSpecialPushForces ? tramp.SpecialBallPushForce : tramp.PushForce;

        ball.Rigidbody.AddForce(force * trampTransform.up, ForceMode.Impulse);
                
        otherEntity.Get<TrampolineTimeOut>() = new TrampolineTimeOut()
        {
            Time = 0.3f,
            TrampolineEntity = trampolineEntity,
        };

        JumpAnimation(tramp.Animator);
        tramp.JumpFx.Play();
    }

    private void JumpAnimation(Animator animator)
    {
        animator.Play("tram_1_action_1");
    }
}

public struct TrampolineCollision
{
    public EcsEntity TrampolineEntity;
    public EcsEntity OtherEntity;
}


