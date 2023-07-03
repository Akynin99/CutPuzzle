using UnityEngine;
using Leopotam.Ecs;
using Modules.EventGroup;

namespace Modules.CutPuzzle.EventHandlers
{
    /// <summary>
    /// process enter at a regular gameplay state(after click on TTS)
    /// </summary>
    public class OnGamePlayStateEnter : IEcsRunSystem
    {
        readonly EcsFilter<GamePlayState, StateEnter> _signal;
        readonly EcsWorld _world;

        public void Run()
        {
            if (_signal.IsEmpty())
                return;

            // show gameplay screen
            ref var screen = ref _world.NewEntity().Get<UICoreECS.ShowScreenTag>();
            screen.ID = (int)UI.MainScreens.GamePlay;
            screen.Layer = (int)UI.Layers.MainLayer;
        }
    }
}