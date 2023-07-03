using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class PushRagdollSystem : IEcsRunSystem
{
    private EcsFilter<RagdollComponent, PushRagdoll> _filter;

    private EcsWorld _world;
    private TimeService _timeService;

    public void Run()
    {
        foreach (var i in _filter)
        {
            Vector3 force = _filter.Get2(i).Force;
            foreach (var rigidbody in _filter.Get1(i).Rigidbodies)
            {
                rigidbody.AddForce(force, ForceMode.VelocityChange);
            }
            
            _filter.GetEntity(i).Del<PushRagdoll>();
        }
    }

    
}

public struct PushRagdoll
{
    public Vector3 Force;
}




