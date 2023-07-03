using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.Utils;
using Modules.VibroService;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class SurfaceHitFx : MonoBehaviour
{
    public ParticleSystem Fx;
    public float ImpulseForHit;
    public Rigidbody Rigidbody;
    public bool PlaceFxOnCollisionPoint;
    
    private const float VibroCooldown = 0.5f;

    private Vector3 _lastVelocity;
    private static float _lastVibroTime;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == gameObject.layer)
            return;

        if (other.impulse.magnitude < ImpulseForHit)
            return;

        if (PlaceFxOnCollisionPoint)
            Fx.transform.position = other.GetContact(0).point;
        
        Fx.Play();
        DoVibro();
    }

    private static void DoVibro()
    {
        if(!VibroSettings.VibroEnabled || _lastVibroTime + VibroCooldown > Time.time)
            return;

        MoreMountains.NiceVibrations.MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.MediumImpact);
        
        _lastVibroTime = Time.time;
    }
}
