using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class VictoryFxTemplate : ViewElement
{
    public ParticleSystem ParticleSystem;
    
    public override void OnSpawn(EcsEntity entity, EcsWorld world)
    {
        base.OnSpawn(entity, world);
        
        entity.Get<VictoryFx>() = new VictoryFx()
        {
            ParticleSystem = ParticleSystem
        };
    }
}
public struct VictoryFx
{
    public ParticleSystem ParticleSystem;
}