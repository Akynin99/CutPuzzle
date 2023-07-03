using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.LevelSpawner;
using Modules.UserInput;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class MirroredLevelFixSystem : IEcsRunSystem
{
    private EcsFilter<AddressableLevelAsset, AssetSpawnedTag> _filter;
    private EcsFilter<Human, UnityView>.Exclude<Mirrored> _humans;
    private EcsFilter<Enemy, UnityView>.Exclude<Mirrored> _enemies;

    public void Run()
    {
        if(_filter.IsEmpty())
            return;
        
        if(_humans.IsEmpty() && _enemies.IsEmpty())
            return;

        bool mirrored = false;

        foreach (var i in _filter)
        {
            if (_filter.Get1(i).Mirrored)
                mirrored = true;
        }

        if(!mirrored)
            return;

        foreach (var i in _humans)
        {
            _humans.Get2(i).Transform.localScale = new Vector3(-1,1,1);
        }
        
        foreach (var i in _enemies)
        {
            _enemies.Get2(i).Transform.localScale = new Vector3(-1,1,1);
        }
    }
}

public struct Mirrored : IEcsIgnoreInFilter
{
    
}

