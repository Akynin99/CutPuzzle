using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.CameraUtils;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class LevelConfigSystem : IEcsRunSystem
{
    readonly EcsFilter<LevelConfig> _filter;
    readonly EcsFilter<NextLevelState> _nextLevel;
    readonly EcsFilter<VirtualCamera> _cameras;
    readonly EcsFilter<VictoryFx, UnityView> _victory;

    private EcsWorld _world;

    public void Run()
    {
        if(_filter.IsEmpty())
            return;

        ref var levelConfig = ref _filter.Get1(0);

        foreach (var i in _cameras)
        {
            _cameras.Get1(i).Camera.transform.position = levelConfig.CameraPosition;
            _cameras.Get1(i).Camera.transform.rotation = Quaternion.Euler(levelConfig.CameraRotation);
        }
        
        foreach (var i in _victory)
        {
            _victory.Get2(i).Transform.position = levelConfig.ConfettiPosRelativeToCamera + levelConfig.CameraPosition;
        }

    }

    
}




