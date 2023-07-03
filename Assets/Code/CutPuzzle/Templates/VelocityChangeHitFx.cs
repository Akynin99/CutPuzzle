using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class VelocityChangeHitFx : MonoBehaviour
{
    public ParticleSystem Fx;
    public float VelocityChangeForHit;
    public Rigidbody Rigidbody;
    public float TimeBeforeInit = 1f;

    private Vector3 _lastVelocity;
    private float _timer;
    private bool _inited;

    private void Start()
    {
        _timer = TimeBeforeInit;
    }

    private void Update()
    {
        if(_inited)
            return;

        _timer -= Time.unscaledDeltaTime;

        if (_timer <= 0)
        {
            _inited = true;
            _lastVelocity = Rigidbody.velocity;
        }
    }

    private void FixedUpdate()
    {
        if(!_inited)
            return;
        
        Vector3 velocity = Rigidbody.velocity;

        Vector3 velocityChangeVector = velocity - _lastVelocity;

        float velocityChange = velocityChangeVector.magnitude;

        if (velocityChange >= VelocityChangeForHit)
        {
            Fx.Play();
        }

        _lastVelocity = velocity;
    }

}
