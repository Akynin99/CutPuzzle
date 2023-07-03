using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class CutTrailTemplate : ViewElement
{
    public TrailRenderer TrailRenderer;
    public ParticleSystem ParticleSystem;
    
    public override void OnSpawn(EcsEntity entity, EcsWorld world)
    {
        base.OnSpawn(entity, world);

        entity.Get<CutTrail>() = new CutTrail()
        {
            TrailRenderer = TrailRenderer,
            ParticleSystem = ParticleSystem,
        };
    }


}
public struct CutTrail
{
     public TrailRenderer TrailRenderer;
     public ParticleSystem ParticleSystem;
}