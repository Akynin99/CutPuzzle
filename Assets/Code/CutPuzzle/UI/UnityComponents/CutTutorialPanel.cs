using System;
using Leopotam.Ecs;
using Modules.EventGroup;
using UnityEngine;
using Modules.PlayerLevel;
using TMPro;
using UICoreECS;

namespace Modules.CutPuzzle.UI
{
    public class CutTutorialPanel : AUIEntity
    {
        public CanvasGroup RootCanvasGroup;
        public float HideAlphaChangeRate;
        public float ShowAlphaChangeRate;

        private bool _lastState;
        
        public override void Init(EcsWorld world, EcsEntity screen)
        {
            screen.Get<CutTutorialPanelView>().View = this;
            _lastState = true;
            
            SetTutorialEnable(false);
        }

        public void Update()
        {
            if(_lastState && RootCanvasGroup.alpha >= 1 || !_lastState && RootCanvasGroup.alpha <= 0)
                return;

            if (_lastState)
            {
                if(RootCanvasGroup.alpha >= 1)
                    return;

                RootCanvasGroup.alpha += ShowAlphaChangeRate * Time.unscaledDeltaTime;
            }
            else
            {
                if(RootCanvasGroup.alpha <= 0)
                    return;
                
                RootCanvasGroup.alpha -= HideAlphaChangeRate * Time.unscaledDeltaTime;
            }
        }

        public void SetTutorialEnableSmooth(bool value)
        {
            _lastState = value;
        }
        
        public void SetTutorialEnable(bool value)
        {
            _lastState = value;
            RootCanvasGroup.alpha = value ? 1 : 0;
        }
    }
    
    public struct CutTutorialPanelView
    {
        public CutTutorialPanel View;
    }
}