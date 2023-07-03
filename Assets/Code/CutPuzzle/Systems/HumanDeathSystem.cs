using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.VibroService;
using Obi;
using UnityEngine;

public class HumanDeathSystem : IEcsRunSystem
{
    private EcsFilter<Human, DeathTag> _filter;

    private EcsWorld _world;
    private static readonly int Emotion = Animator.StringToHash("emotion");

    public void Run()
    {
        foreach (var i in _filter)
        {
            // _filter.Get1(i).Renderer.enabled = false;
            _filter.Get1(i).Animator.SetFloat(Emotion, 1f);
            _filter.Get1(i).Collider.enabled = false;
            _filter.Get1(i).DeathFx.Play();

            foreach (var renderer in _filter.Get1(i).MainRenderers)
            {
                renderer.material = _filter.Get1(i).DeathMaterial;
            }
            _filter.GetEntity(i).Del<DeathTag>();
            _filter.GetEntity(i).Del<Human>();
            
            if (VibroSettings.VibroEnabled)
            {
                MoreMountains.NiceVibrations.MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.SoftImpact);
            }
        }
    }
}

public struct DeathTag : IEcsIgnoreInFilter
{
    
}
