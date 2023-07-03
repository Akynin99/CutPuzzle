using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class FightWithHuman : ViewComponent
{
    private EcsEntity _entity;
    private EcsWorld _ecsWorld;

    public override void EntityInit(EcsEntity ecsEntity, EcsWorld ecsWorld, bool parentOnScene)
    {
        _entity = ecsEntity;
        _ecsWorld = ecsWorld;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer != 3)
            return;
        
        if(_entity.Has<InFight>())
            return;

        EntityRef entityRef = null;

        entityRef = other.transform.GetComponentInParent<EntityRef>();

        if (entityRef != null && entityRef.Entity.Has<Human>() && !entityRef.Entity.Has<InFight>())
        {
            entityRef.Entity.Get<InFight>();
            _entity.Get<InFight>();
            _ecsWorld.NewEntity().Get<Fight>() = new Fight()
            {
                HumanFighter = entityRef.Entity,
                EnemyFighter = _entity,
            };
        }
    }

    
}
