using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class PlatformMoveSystem : IEcsRunSystem
{
    private EcsFilter<OnScreenHold> _hold;
    private EcsFilter<ManIsDead> _manDead;
    private EcsFilter<ManIsNotAttachedTag> _notAttached;
    private EcsFilter<GamePlayState> _gameplay;
    private EcsFilter<MovablePlatform, UnityView, PlatformHold> _platforms;

    private EcsWorld _world;
    private bool _lastFrameHold;
    private readonly float _mouseMult;

    public PlatformMoveSystem()
    {
        _mouseMult = Screen.width / 5f;
    }

    public void Run()
    {
        if(_gameplay.IsEmpty() || !_manDead.IsEmpty() || !_notAttached.IsEmpty())
            return;
        
        if (_hold.IsEmpty())
        {
            if (_platforms.IsEmpty())
            {
                // do nothing
            }
            else
            {
                foreach (var i in _platforms)
                {
                    _platforms.GetEntity(i).Del<PlatformHold>();
                }
            }
        }
        else
        {
            if (_platforms.IsEmpty())
            {
                if (_lastFrameHold)
                {
                    // nothing
                }
                else
                {
                    TryHoldPlatform();
                }
            }
            else
            {
                PlatformMove();
            }
        }

        _lastFrameHold = !_hold.IsEmpty();
    }

    private void TryHoldPlatform()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("MovablePlatform");

        if (Physics.Raycast(ray, out hit, 50, layerMask))
        {
            EntityRef entityRef = null;

            entityRef = hit.collider.GetComponentInParent<EntityRef>();

            if (entityRef != null)
                entityRef.Entity.Get<PlatformHold>().LastMousePos = Input.mousePosition;
        }
    }

    private void PlatformMove()
    {
        Vector3 mousePos = Input.mousePosition;
        
        foreach (var i in _platforms)
        {
            Vector3 mouseDiff = (mousePos - _platforms.Get3(i).LastMousePos) / _mouseMult;
            Vector3 moveVector = (_platforms.Get1(i).MoveEnd2 - _platforms.Get1(i).MoveEnd1).normalized;

            float moveDist = Vector3.Dot(moveVector, mouseDiff);

            if (_platforms.Get1(i).ClampMoveSpeed)
                moveDist = Mathf.Clamp(moveDist, -_platforms.Get1(i).MaxMoveSpeed, _platforms.Get1(i).MaxMoveSpeed);

            float distToEdge = Vector3.Distance(_platforms.Get2(i).Transform.position,
                moveDist > 0 ? _platforms.Get1(i).MoveEnd2 : _platforms.Get1(i).MoveEnd1);

            float distTraveled = 0;
            
            if (distToEdge < Mathf.Abs(moveDist))
            {
                _platforms.Get2(i).Transform.position =
                    moveDist > 0 ? _platforms.Get1(i).MoveEnd2 : _platforms.Get1(i).MoveEnd1;

                distTraveled = distToEdge;
            }
            else
            {
                _platforms.Get2(i).Transform.position += moveVector * moveDist;
                distTraveled = Mathf.Abs(moveDist);
            }

            _platforms.Get1(i).DistanceTraveled += distTraveled;

            _platforms.Get3(i).LastMousePos = mousePos;
        }
    }
}

public struct PlatformHold
{
    public Vector3 LastMousePos;
}



