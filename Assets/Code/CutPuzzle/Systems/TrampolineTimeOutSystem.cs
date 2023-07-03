using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class TrampolineTimeOutSystem : IEcsRunSystem
{
    
    private EcsFilter<TrampolineTimeOut> _filter;

    private EcsWorld _world;
    private TimeService _timeService;

    public void Run()
    {
        foreach (var i in _filter)
        {
            _filter.Get1(i).Time -= _timeService.DeltaTime;
            
            if(_filter.Get1(i).Time <= 0)
                _filter.GetEntity(i).Del<TrampolineTimeOut>();
        }
    }

}


public struct TrampolineTimeOut
{
    public float Time;
    public EcsEntity TrampolineEntity;
}

