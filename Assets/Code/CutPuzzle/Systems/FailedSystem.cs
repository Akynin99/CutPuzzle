using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class FailedSystem : IEcsRunSystem
{
    private EcsFilter<RopeView> _ropes;
    private EcsFilter<GamePlayState> _gameplay;
    private EcsFilter<RoundCompletedState> _completed;
    private EcsFilter<RoundFailedState> _failed;
    private EcsFilter<ManIsNotAttachedTag> _notAttached;
    private EcsFilter<Human> _human;
    private EcsFilter<ManIsDead> _manDead;
    private EcsFilter<Enemy> _enemies;
    private EcsFilter<LevelConfig> _levelConfig;

    private EcsWorld _world;
    private TimeService _timeService;

    private TimingsConfig _timingsConfig;

    public FailedSystem(TimingsConfig timingsConfig)
    {
        _timingsConfig = timingsConfig;
    }

    public void Run()
    {
        if (_gameplay.IsEmpty() || !_completed.IsEmpty() || !_failed.IsEmpty())
        {
            DeleteTags();
            return;
        }

        CheckManDeath();

        if (_notAttached.IsEmpty())
            CheckAttach();
        else
            NotAttachTimer();

    }

    private void CheckAttach()
    {
        bool notAttachedLoose = !(!_levelConfig.IsEmpty() && _levelConfig.Get1(0).NotAttachedLoose == false);

        foreach (var ropeIdx in _ropes)
        {
            if(_ropes.Get1(ropeIdx).AttachedEntity.IsNull())
                continue;
            
            if(_ropes.Get1(ropeIdx).AttachedEntity.Has<Human>())
                return;
        }

        _world.NewEntity().Get<ManIsNotAttachedTag>().Timer = notAttachedLoose
            ? _timingsConfig.FailedAfterCutAllRopesTime
            : _timingsConfig.FailedAfterCutAllRopesTime * 5;
    }

    private void NotAttachTimer()
    {
        foreach (var i in _notAttached)
        {
            _notAttached.Get1(i).Timer -= _timeService.DeltaTime;

            if (_notAttached.Get1(i).Timer <= 0)
            {
                StateFactory.CreateState<RoundFailedState>(_world);
                DeleteTags();
                return;
            }
        }
    }

    private void DeleteTags()
    {
        foreach (var i in _notAttached)
        {
            _notAttached.GetEntity(i).Del<ManIsNotAttachedTag>();
        }
        
        foreach (var i in _manDead)
        {
            _manDead.GetEntity(i).Del<ManIsDead>();
        }
    }

    private void CheckManDeath()
    {
        if (_human.IsEmpty() && _manDead.IsEmpty())
        {
            _world.NewEntity().Get<ManIsDead>().Timer = _timingsConfig.FailedAfterPlayerDeadTime;
            EnemyDance();
        }

        if (_human.IsEmpty())
        {
            foreach (var i in _manDead)
            {
                _manDead.Get1(i).Timer -= _timeService.DeltaTime;
                
                if(_manDead.Get1(i).Timer <= 0)
                {
                    StateFactory.CreateState<RoundFailedState>(_world);
                    DeleteTags();
                    return;
                }
            }
        }
        
        if(!_human.IsEmpty())
            foreach (var i in _manDead)
            {
                _manDead.GetEntity(i).Del<ManIsDead>();
            }
    }

    private void EnemyDance()
    {
        foreach (var i in _enemies)
        {
            _enemies.Get1(i).Animator.SetBool("win", true);
        }
    }
}

public struct ManIsNotAttachedTag
{
    public float Timer;
}

public struct ManIsDead
{
    public float Timer;
}



