using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class TrampolineMoveSystem : IEcsRunSystem
{
    private EcsFilter<OnScreenHold> _hold;
    private EcsFilter<ManIsDead> _manDead;
    private EcsFilter<ManIsNotAttachedTag> _notAttached;
    private EcsFilter<GamePlayState> _gameplay;
    private EcsFilter<Trampoline, UnityView, TrampolineHold> _trampolines;

    private EcsWorld _world;
    private bool _lastFrameHold;
    private readonly float _mouseMult;

    public TrampolineMoveSystem()
    {
        _mouseMult = Screen.width / 5f;
    }

    public void Run()
    {
        if(_gameplay.IsEmpty() || !_manDead.IsEmpty())
            return;
        
        // if(!_notAttached.IsEmpty())
        //     return;
        
        if (_hold.IsEmpty())
        {
            if (_trampolines.IsEmpty())
            {
                // do nothing
            }
            else
            {
                // del TrampolineHold components

                foreach (var i in _trampolines)
                {
                    _trampolines.GetEntity(i).Del<TrampolineHold>();
                }
            }
        }
        else
        {
            if (_trampolines.IsEmpty())
            {
                if (_lastFrameHold)
                {
                    // nothing
                }
                else
                {
                    // try hold trampoline
                    TryHoldTrampoline();
                }
            }
            else
            {
                // move trampoline
                TrampolineMove();
            }
        }

        _lastFrameHold = !_hold.IsEmpty();
    }

    private void TryHoldTrampoline()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Trampoline");

        if (Physics.Raycast(ray, out hit, 50, layerMask))
        {
            EntityRef entityRef = null;

            entityRef = hit.collider.GetComponentInParent<EntityRef>();

            if (entityRef != null)
                entityRef.Entity.Get<TrampolineHold>().LastMousePos = Input.mousePosition;
        }
    }

    private void TrampolineMove()
    {
        Vector3 mousePos = Input.mousePosition;
        
        foreach (var i in _trampolines)
        {
            Vector3 mouseDiff = (mousePos - _trampolines.Get3(i).LastMousePos) / _mouseMult;
            Vector3 trampolineMoveVector = (_trampolines.Get1(i).MoveEnd2 - _trampolines.Get1(i).MoveEnd1).normalized;

            float moveDist = Vector3.Dot(trampolineMoveVector, mouseDiff);

            float distToEdge = Vector3.Distance(_trampolines.Get2(i).Transform.position,
                moveDist > 0 ? _trampolines.Get1(i).MoveEnd2 : _trampolines.Get1(i).MoveEnd1);

            float distTraveled = 0;
            
            if (distToEdge < Mathf.Abs(moveDist))
            {
                _trampolines.Get2(i).Transform.position =
                    moveDist > 0 ? _trampolines.Get1(i).MoveEnd2 : _trampolines.Get1(i).MoveEnd1;

                distTraveled = distToEdge;
            }
            else
            {
                _trampolines.Get2(i).Transform.position += trampolineMoveVector * moveDist;
                distTraveled = Mathf.Abs(moveDist);
            }

            _trampolines.Get1(i).DistanceTraveled += distTraveled;

            _trampolines.Get3(i).LastMousePos = mousePos;
        }
    }
}

public struct TrampolineHold
{
    public Vector3 LastMousePos;
}



