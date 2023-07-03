using Leopotam.Ecs;
using Modules.Root.ECS;
using Modules.Root.ContainerComponentModel;
using UnityEngine;

namespace Modules.CutPuzzle
{
    [CreateAssetMenu(menuName = "Modules/CT/Provider")]
    public class CPProvider : ScriptableObject, ISystemsProvider
    {
        [SerializeField] private TimingsConfig timingsConfig;
        public EcsSystems GetSystems(EcsWorld world, EcsSystems endFrame, EcsSystems mainSystems)
        {
            EcsSystems systems = new EcsSystems(world, "CTGame");

            #region AppContainerCheck
            if (AppContainer.Instance == null) 
            {
                // wrong behavior
                // app container not initialized, handle via init scene load
                Debug.LogWarning(
                    "<color=darkblue>CommonTemplate:</color> App container not initialized, resolved via init scene load\n" +
                    "LOOK AT: http://youtrack.lipsar.studio/articles/LS-A-149/App-container-not-initialized"
                    );

#if UNITY_IOS || UNITY_ANDROID
                Handheld.Vibrate();
#endif

                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                return systems;
            }
            #endregion

            systems
                .Add(new GameInit())
                
                .Add(new DisableTTSSystem())
                .Add(new MirroredLevelFixSystem())

                .Add(new LevelConfigSystem())
                .Add(new RopeInitSystem())
                .Add(new FailedSystem(timingsConfig))
                .Add(new CutTutorialSystem())
                .Add(new MoveTutorialSystem())
                
                .Add(new BombCollisionSystem())
                .Add(new SpikesCollisionBombSystem())
                .Add(new SpikesCollisionHumanSystem())
                .Add(new SpikesCollisionEnemySystem())
                .Add(new BallCollisionBombSystem())
                .Add(new BallCollisionSpikesSystem())
                .Add(new BallCollisionEnemySystem())
                .Add(new BallCollisionHumanSystem())
                .Add(new PlatformCollisionEnemySystem())
                
                .Add(new FightSystem())
                
                .Add(new CutDebugSystem())
                .Add(new CutTrailSystem())
                .Add(new CutSystem())
                .Add(new HumanDeathSystem())
                .Add(new EnemyDeathSystem())
                .Add(new HumanWinSystem(timingsConfig))
                .Add(new TrampolineTimeOutSystem())
                .Add(new PlatformMoveSystem())
                .Add(new TrampolineMoveSystem())
                .Add(new TrampolinePushSystem())
                .Add(new BombDetonateSystem())
                .Add(new HoleAttractionSystem())
                
                .Add(new FreezeRagdollSystem())
                .Add(new PushRagdollSystem())

                // event group
                .Add(new EventGroup.StateCleanupSystem())       // remove entity with prev state component
                .Add(new EventHandlers.OnRestartRoundEnter())   // on click at restart button
                .Add(new EventHandlers.OnNextLevelEnter())      // start next level
                .Add(new EventHandlers.OnGamePlayStateEnter())  // enter at gameplay stage
                .Add(new EventHandlers.OnRoundCompletedEnter()) // on round completed state enter
                .Add(new EventHandlers.OnRoundFailedEnter())    // on round failed state enter

                .Add(new Utils.TimedDestructorSystem());

            endFrame
                .OneFrame<BombCollision>()
                .OneFrame<BallCollision>()
                .OneFrame<PlatformCollision>()
                .OneFrame<SpikeCollision>()
                .OneFrame<EventGroup.StateEnter>();

            return systems;
        }
    }
}
