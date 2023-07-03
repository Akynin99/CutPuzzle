using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class RopeTemplate : ViewElement
{
    public ObiRope Rope;
    
    public override void OnSpawn(EcsEntity entity, EcsWorld world)
    {
        base.OnSpawn(entity, world);
        
        entity.Get<RopeView>().View = Rope;
    }


}
public struct RopeView
{
    public ObiRope View;
    public EcsEntity AttachedEntity;
    public bool Inited;
}