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

public class CutTutorialSystem : IEcsRunSystem
{
    private EcsFilter<CutTutorialPanelView> _tutorials;
    private EcsFilter<OnScreenHold> _hold;
    private EcsFilter<ManIsNotAttachedTag> _notAttached;

    private EcsWorld _world;
    private TimeService _timeService;

    public void Run()
    {
        if (ProgressionInfo.CurrentLevel > 0)
        {
            DisableTutorialFast();
        }
        else
        {
            if (_hold.IsEmpty() && _notAttached.IsEmpty())
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





