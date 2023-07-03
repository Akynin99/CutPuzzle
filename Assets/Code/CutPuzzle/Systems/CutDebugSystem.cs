using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Obi;
using UnityEngine;

public class CutDebugSystem : IEcsRunSystem
{
    private EcsFilter<CutLine> _cutLines;
    private EcsFilter<CutDebugLine> _filter;

    private EcsWorld _world;

    public void Run()
    {
        
        foreach (var i in _cutLines)
        {
            foreach (var j in _filter)
            {
                _filter.Get1(j).Renderer.SetPosition(0, _cutLines.Get1(i).StartPoint);
                _filter.Get1(j).Renderer.SetPosition(1, _cutLines.Get1(i).EndPoint);
            }
        }
    }

    

    
}

