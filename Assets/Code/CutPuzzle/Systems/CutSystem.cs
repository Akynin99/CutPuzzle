using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UPhysics;
using Modules.UserInput;
using Modules.VibroService;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class CutSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsFilter<OnScreenHold> _hold;
    private EcsFilter<CutLine> _lines;
    private EcsFilter<RopeView> _ropes;
    private EcsFilter<RoundFailedState> _failed;
    private EcsFilter<RoundCompletedState> _completed;
    private EcsFilter<PlatformHold> _platformHold;

    private EcsWorld _world;
    private IEcsInitSystem _ecsInitSystemImplementation;
    private LayerMask _raycastMask;
    private Camera _mainCamera;
    
    public void Init()
    {
        _raycastMask = LayerMask.GetMask(LayerList.RaycastSurface.ToString());
        _mainCamera = Camera.main;
    }

    public void Run()
    {
        if(!_failed.IsEmpty() || !_completed.IsEmpty())
            return;

        if (!_platformHold.IsEmpty())
            return;
        
        if (_hold.IsEmpty())
        {
            if (_lines.IsEmpty())
            {
                // nothing happens
            }
            else
            {
                // do cut
                
                foreach (var i in _lines)
                {
                    Cut(_lines.Get1(i).StartPoint, _lines.Get1(i).EndPoint);
                    _lines.GetEntity(i).Del<CutLine>();
                }
            }
        }
        else
        {
            Vector3 point = ScreenToRopeSpacePos(Input.mousePosition);
            
            if (_lines.IsEmpty())
            {
                // start CutLine

                ref var line = ref _world.NewEntity().Get<CutLine>();
                line.StartPoint = point;
                line.EndPoint = point;
            }
            else
            {
                // cut and change CutLine

                foreach (var i in _lines)
                {
                    // _lines.Get1(i).EndPoint = point;
                    Cut(_lines.Get1(i).StartPoint, point);
                    _lines.Get1(i).StartPoint = _lines.Get1(i).EndPoint;
                }
                
                
                
                foreach (var i in _lines)
                {
                    Cut(_lines.Get1(i).StartPoint, _lines.Get1(i).EndPoint);
                    _lines.Get1(i).EndPoint = point;
                }
            }
        }
    }

    private Vector3 ScreenToRopeSpacePos(Vector3 point)
    {
        Vector3 pos = Vector3.zero;
        Ray ray = _mainCamera.ScreenPointToRay(point);
        RaycastHit hit;
        
        if(Physics.Raycast(ray, out hit, 40, _raycastMask))
        {
            pos = hit.point;
        }


        return pos;
    }

    private void Cut(Vector3 cutStart, Vector3 cutEnd)
    {
        bool doVibro = false;
        foreach (var idx in _ropes)
        {
            ObiRope rope = _ropes.Get1(idx).View;

            for (var i = 1; i < rope.elements.Count - 1; i++)
            {
                Vector3 particle1 = rope.GetParticlePosition(rope.elements[i].particle1);
                Vector3 particle2 = rope.GetParticlePosition(rope.elements[i].particle2);

                if (AreCrossing(cutStart, cutEnd, particle1, particle2))
                {
                    rope.Tear(rope.elements[i]);
                    rope.RebuildConstraintsFromElements();

                    EntityRef entityRef = null;
                    entityRef = rope.GetComponent<EntityRef>();

                    if (entityRef != null && entityRef.Entity.Has<RopeView>())
                    {
                        ref RopeView ropeView = ref entityRef.Entity.Get<RopeView>();
                        
                        if(ropeView.AttachedEntity.IsNull())
                            continue;
                        
                        if(!ropeView.AttachedEntity.Has<Bomb>())
                            ropeView.AttachedEntity = EcsEntity.Null;
                    }

                    doVibro = true;
                }
            }
        }
        
        if (doVibro && VibroSettings.VibroEnabled)
        {
            MoreMountains.NiceVibrations.MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.MediumImpact);
        }
    }

    private bool AreCrossing(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        float v1 = VectorMult(p4.x - p3.x, p4.y - p3.y, p1.x - p3.x, p1.y - p3.y);
        float v2 = VectorMult(p4.x - p3.x, p4.y - p3.y, p2.x - p3.x, p2.y - p3.y);
        float v3 = VectorMult(p2.x - p1.x, p2.y - p1.y, p3.x - p1.x, p3.y - p1.y);
        float v4 = VectorMult(p2.x - p1.x, p2.y - p1.y, p4.x - p1.x, p4.y - p1.y);
        if ( (v1*v2)<0 && (v3*v4)<0 )
            return true;
        return false;
    }
    
    private float VectorMult(float ax,float ay,float bx,float by) 
    {
        return ax*by-bx*ay;
    }
}

public struct CutLine
{
    public Vector3 StartPoint;
    public Vector3 EndPoint;
}
