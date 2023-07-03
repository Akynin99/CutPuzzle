using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class BallCollisionSpikesSystem : IEcsRunSystem
{
    private EcsFilter<Spikes, BallCollision> _filter;

    private EcsWorld _world;

    public void Run()
    {
        foreach (var i in _filter)
        {
            if(!_filter.Get1(i).Destructable)
                continue;
            _filter.Get1(i).Visuals.SetActive(false);
            _filter.Get1(i).Collider.enabled = false;
            _filter.GetEntity(i).Del<Spikes>();
        }
    }
}



