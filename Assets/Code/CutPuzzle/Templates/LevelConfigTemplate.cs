using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.Utils;
using Modules.ViewHub;
using UnityEngine;

public class LevelConfigTemplate : ViewElement
{
    public Vector3 CameraPosition;
    public Vector3 CameraRotation;
    public Vector3 ConfettiPosRelativeToCamera;
    public bool NotAttachedLoose;
    
    public override void OnSpawn(EcsEntity entity, EcsWorld world)
    {
        base.OnSpawn(entity, world);
        
        entity.Get<LevelConfig>() = new LevelConfig()
        {
            CameraPosition = CameraPosition,
            CameraRotation = CameraRotation,
            ConfettiPosRelativeToCamera = ConfettiPosRelativeToCamera,
            NotAttachedLoose = NotAttachedLoose,
        };
    }


}
public struct LevelConfig
{
    public Vector3 CameraPosition;
    public Vector3 CameraRotation;
    public Vector3 ConfettiPosRelativeToCamera;
    public bool NotAttachedLoose;
}