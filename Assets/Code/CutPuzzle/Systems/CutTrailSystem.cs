using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class CutTrailSystem : IEcsRunSystem
{
    private EcsFilter<CutLine> _cutLines;
    private EcsFilter<CutTrail, UnityView> _filter;

    private EcsWorld _world;
    private bool _lastFrameHold;

    public void Run()
    {
        foreach (var i in _cutLines)
        {
            foreach (var j in _filter)
            {
                _filter.Get2(j).Transform.position = _cutLines.Get1(i).EndPoint;
                _filter.Get1(j).ParticleSystem.gameObject.SetActive(true);
                _filter.Get1(j).TrailRenderer.gameObject.SetActive(true);

                if (!_lastFrameHold)
                {
                    _filter.Get1(j).TrailRenderer.Clear();
                    // _filter.Get1(j).ParticleSystem.Clear();
                }
            }
        }

        if (!_lastFrameHold && _cutLines.IsEmpty())
        {
            foreach (var j in _filter)
            {
                _filter.Get1(j).ParticleSystem.gameObject.SetActive(false);
                // _filter.Get1(j).TrailRenderer.gameObject.SetActive(false);
            }
        }

        _lastFrameHold = !_cutLines.IsEmpty();
    }

    

    
}

