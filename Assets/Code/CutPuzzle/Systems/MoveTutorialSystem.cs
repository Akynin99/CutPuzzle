using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.CutPuzzle.UI;
using Modules.EventGroup;
using Modules.PlayerLevel;
using Modules.UserInput;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class MoveTutorialSystem : IEcsRunSystem
{
    private EcsFilter<MoveTutorialPanelView> _tutorials;
    private EcsFilter<OnScreenHold> _hold;
    private EcsFilter<Trampoline> _trampolines;

    private EcsWorld _world;
    private TimeService _timeService;

    public void Run()
    {
        if (ProgressionInfo.CurrentLevel > 2)
        {
            DisableTutorialFast();
        }
        else
        {
            if(_trampolines.IsEmpty())
                DisableTutorialFast();

            float maxDist = 0;

            foreach (var i in _trampolines)
            {
                if (_trampolines.Get1(i).DistanceTraveled > maxDist)
                    maxDist = _trampolines.Get1(i).DistanceTraveled;
            }
            
            if (_hold.IsEmpty() && maxDist < 1.5f)
                ShowTutorialSmooth();
            else
                DisableTutorialSmooth();

        }

    }

    private void DisableTutorialFast()
    {
        foreach (var i in _tutorials)
        {
            _tutorials.Get1(i).View.SetTutorialEnable(false);
        }
    }
    
    private void DisableTutorialSmooth()
    {
        foreach (var i in _tutorials)
        {
            _tutorials.Get1(i).View.SetTutorialEnableSmooth(false);
        }
    }
    
    private void ShowTutorialSmooth()
    {
        foreach (var i in _tutorials)
        {
            _tutorials.Get1(i).View.SetTutorialEnableSmooth(true);
        }
    }
    
}





