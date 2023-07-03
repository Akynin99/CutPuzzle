using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.LevelSpawner;
using Modules.UserInput;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class DisableTTSSystem : IEcsRunSystem
{
    private EcsFilter<NextLevelState> _nextLevel;
    private EcsFilter<LevelSpawnedSignal> _spawnedSignal;

    private EcsWorld _world;

    public void Run()
    {
        if(_nextLevel.IsEmpty())
            return;

        if (!_spawnedSignal.IsEmpty())
            StateFactory.CreateState<GamePlayState>(_world);
    }
}

