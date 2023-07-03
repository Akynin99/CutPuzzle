using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class HoleAttractionSystem : IEcsRunSystem
{
    private EcsFilter<Human, RagdollComponent> _humans;
    private EcsFilter<RopeView> _ropes;
    private EcsFilter<Hole> _holes;
    private EcsFilter<ManIsNotAttachedTag> _notAttached;
    
    private EcsFilter<GamePlayState> _gameplay;

    private EcsWorld _world;

    public void Run()
    {
        if (_gameplay.IsEmpty())
        {
            // foreach (var holeIdx in _holes)
            // {
            //     _holes.Get1(holeIdx).PullingFx.Stop();
            // }
            
            return;
        }
        
        if(_notAttached.IsEmpty() || _humans.IsEmpty())
            return;
        
        foreach (var holeIdx in _holes)
        {
            ref var hole = ref _holes.Get1(holeIdx);
            Transform[] holePoints = hole.AttractingPoints;
            Vector3 point = holePoints[hole.PointIndex].position;
            
            if (hole.PullingFx && !hole.PullingFx.isPlaying)
            {
                hole.PullingFx.Play();
                hole.TubeFx.gameObject.SetActive(false);
            }

            foreach (var manIdx in _humans)
            {
                ref var man = ref _humans.Get1(manIdx);
                var bones = man.BonesForAttraction;
                foreach (var bone in bones)
                {
                    Vector3 bonePos = bone.transform.position;
                    
                    float dist = Vector3.Distance(point, bonePos);

                    if (hole.PointIndex < (holePoints.Length - 1) && dist < 0.5f)
                    {
                        hole.PointIndex += 1;
                        point = holePoints[hole.PointIndex].position;
                    }
                
                    if(hole.Pulling == false && hole.PointIndex == 0 && dist > hole.FirstPointRadius)
                        continue;

                    hole.Pulling = true;
                    if (hole.PullingFx && !hole.PullingFx.isPlaying)
                    {
                        hole.PullingFx.Play();
                        hole.TubeFx.gameObject.SetActive(false);
                    }

                    Vector3 dirToPoint = (point - bonePos).normalized;
                    Vector3 force = dirToPoint * 160f;
                    bone.AddForce(force, ForceMode.Acceleration);
                }
                
            }
        }
    }
}

