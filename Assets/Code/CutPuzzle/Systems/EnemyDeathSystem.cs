using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.Utils;
using Obi;
using UnityEngine;

public class EnemyDeathSystem : IEcsRunSystem
{
    private EcsFilter<Enemy, DeathTag> _filter;

    private EcsWorld _world;

    public void Run()
    {
        foreach (var i in _filter)
        {
            _filter.Get1(i).Collider.enabled = false;

            foreach (var renderer in _filter.Get1(i).MainRenderers)
            {
                renderer.material = _filter.Get1(i).DeathMaterial;
            }

            if (_filter.GetEntity(i).Has<RagdollComponent>())
            {
                ref var ragdoll = ref _filter.GetEntity(i).Get<RagdollComponent>();
                ragdoll.SetMode(true);
                foreach (var rigidbody in ragdoll.Rigidbodies)
                {
                    rigidbody.gameObject.layer = LayerMask.NameToLayer("Default");
                }
            }

            foreach (var emote in _filter.Get1(i).AliveEmotes)
            {
                emote.SetActive(false);
            }
            
            foreach (var emote in _filter.Get1(i).DeadEmotes)
            {
                emote.SetActive(true);
            }
            
            _filter.GetEntity(i).Del<DeathTag>();
            _filter.GetEntity(i).Del<Enemy>();
            
        }
    }
}

