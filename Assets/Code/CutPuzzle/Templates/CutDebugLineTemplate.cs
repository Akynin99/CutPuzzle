using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class CutDebugLineTemplate : ViewElement
{
    public LineRenderer Renderer;
    
    public override void OnSpawn(EcsEntity entity, EcsWorld world)
    {
        base.OnSpawn(entity, world);

        entity.Get<CutDebugLine>() = new CutDebugLine()
        {
            Renderer = Renderer,
        };
    }


}
public struct CutDebugLine
{
    public LineRenderer Renderer;
}