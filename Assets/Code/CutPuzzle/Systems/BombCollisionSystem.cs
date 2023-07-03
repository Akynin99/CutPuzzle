using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class BombCollisionSystem : IEcsRunSystem
{
    private EcsFilter<Bomb, BombCollision> _filter;

    private EcsWorld _world;

    public void Run()
    {
        foreach (var i in _filter)
        {
            if (_filter.Get2(i).Impulse > _filter.Get1(i).DetonateImpulse)
                _filter.GetEntity(i).Get<BombDetonate>();
        }
    }


}

public struct BombCollision
{
    public float Impulse;
}


