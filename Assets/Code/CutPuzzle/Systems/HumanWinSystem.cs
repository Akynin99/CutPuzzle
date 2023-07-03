using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.Utils;
using Obi;
using UnityEngine;

public class HumanWinSystem : IEcsRunSystem
{
    private EcsFilter<Human, HoleTouched>.Exclude<HumanSaved> _filter;
    private EcsFilter<Human, HumanSaved> _saved;
    private EcsFilter<GamePlayState> _gameplay;
    readonly EcsFilter<VictoryFx> _victoryFx;

    private EcsWorld _world;
    private TimeService _timeService;
    private TimingsConfig _timingsConfig;

    public HumanWinSystem(TimingsConfig timingsConfig)
    {
        _timingsConfig = timingsConfig;
    }

    public void Run()
    {
        if(_gameplay.IsEmpty())
            return;
        
        foreach (var i in _filter)
        {
            _filter.GetEntity(i).Get<HumanSaved>().Timer = _timingsConfig.CompletedUITime;
            _filter.GetEntity(i).Get<FreezeTag>();
            _filter.GetEntity(i).Del<HoleTouched>();
            
            foreach (var j in _victoryFx)
            {
                _victoryFx.Get1(j).ParticleSystem.Play();
            }
        }

        foreach (var i in _saved)
        {
            _saved.Get2(i).Timer -= _timeService.DeltaTime;
            
            if(_saved.Get2(i).Timer > 0)
                continue;
            
            StateFactory.CreateState<RoundCompletedState>(_world);
        }
    }

    

    
}

public struct HoleTouched : IEcsIgnoreInFilter
{
    
}

public struct HumanSaved 
{
    public float Timer;
}
