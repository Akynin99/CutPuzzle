using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.Utils;
using Modules.VibroService;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class BallCollisionHumanSystem : IEcsRunSystem
{
    private EcsFilter<Human, BallCollision> _filter;

    private EcsWorld _world;

    public void Run()
    {
        foreach (var i in _filter)
        {
            if(!_filter.Get2(i).CanKillPlayer)
                continue;

            _filter.GetEntity(i).Get<DeathTag>();
            _filter.GetEntity(i).Del<SpikeCollision>();
        }
    }
}



