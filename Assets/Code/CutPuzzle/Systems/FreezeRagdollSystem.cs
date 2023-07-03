using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class FreezeRagdollSystem : IEcsRunSystem
{
    private EcsFilter<RagdollComponent, FreezeTag> _freezeFilter;
    private EcsFilter<RagdollComponent, UnFreezeTag> _unfreezeFilter;

    private EcsWorld _world;
    private TimeService _timeService;

    public void Run()
    {
        foreach (var i in _freezeFilter)
        {
            foreach (var rigidbody in _freezeFilter.Get1(i).Rigidbodies)
            {
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
                rigidbody.velocity = Vector3.zero;
            }
            
            foreach (var collider in _freezeFilter.Get1(i).Colliders)
            {
                collider.enabled = false;
            }
            
            _freezeFilter.GetEntity(i).Del<FreezeTag>();
        }
        
        foreach (var i in _unfreezeFilter)
        {
            foreach (var rigidbody in _unfreezeFilter.Get1(i).Rigidbodies)
            {
                rigidbody.useGravity = true;
                rigidbody.isKinematic = false;
                rigidbody.velocity = Vector3.zero;
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
            
            foreach (var collider in _freezeFilter.Get1(i).Colliders)
            {
                collider.enabled = true;
            }
            
            _unfreezeFilter.GetEntity(i).Del<UnFreezeTag>();
        }
    }

    
}

public struct FreezeTag
{
    
}

public struct UnFreezeTag
{
    
}




