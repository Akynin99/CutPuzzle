using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class FightSystem : IEcsRunSystem
{
    private EcsFilter<Fight> _fights;

    private EcsWorld _world;
    private TimeService _timeService;

    public void Run()
    {
        foreach (var i in _fights)
        {
            ref var fight = ref _fights.Get1(i);
            if(!fight.Init)
                continue;

            fight.Timer -= _timeService.DeltaTime;

            if (fight.Timer <= 0)
            {
                Vector3 playerPos = fight.HumanFighter.Get<Human>().BoneForPosition.position;
                Vector3 enemyPos = fight.EnemyFighter.Get<Enemy>().BoneForPosition.position;
                Vector3 forceDir = (playerPos - enemyPos).normalized;
                Vector3 force = forceDir * 10f;
                
                fight.HumanFighter.Get<DeathTag>();
                fight.HumanFighter.Get<UnFreezeTag>();
                fight.HumanFighter.Get<PushRagdoll>().Force = force;
                
                fight.HumanFighter.Del<InFight>();
                fight.EnemyFighter.Del<InFight>();
                _fights.GetEntity(i).Del<Fight>();
            }
        }

        InitFightsRun();
    }

    private void InitFightsRun()
    {
        foreach (var i in _fights)
        {
            ref var fight = ref _fights.Get1(i);
            if(fight.Init)
                continue;

            fight.Init = true;

            if (!fight.HumanFighter.Has<Human>())
            {
                Debug.LogError("Can't find human in fight");
            }
            else
            {
                fight.HumanFighter.Get<Human>().FightFx.Play();
                fight.HumanFighter.Get<FreezeTag>();
            }

            if (!fight.EnemyFighter.Has<Enemy>())
            {
                Debug.LogError("Can't find enemy in fight");
            }
            else
            {
                fight.EnemyFighter.Get<Enemy>().FightFx.Play();
                fight.EnemyFighter.Get<Enemy>().Animator.SetTrigger("attack");
            }

            fight.Timer = 0.5f;
        }
    }
}

public struct Fight
{
    public EcsEntity HumanFighter;
    public EcsEntity EnemyFighter;
    public bool Init;
    public float Timer;
}

public struct InFight
{
    
}


