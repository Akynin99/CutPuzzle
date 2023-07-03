using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class SpikesCollisionHumanSystem : IEcsRunSystem
{
    private EcsFilter<Human, SpikeCollision> _filter;

    private EcsWorld _world;

    public void Run()
    {
        foreach (var i in _filter)
        {
            _filter.GetEntity(i).Get<DeathTag>();
            _filter.GetEntity(i).Del<SpikeCollision>();
        }
    }


}

public struct SpikeCollision
{
}


