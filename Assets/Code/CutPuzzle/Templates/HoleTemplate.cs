using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class HoleTemplate : ViewComponent
{
    public Transform[] AttractingPoints;
    public float FirstPointRadius;
    public ParticleSystem PullingFx;
    public ParticleSystem TubeFx;
    
    public override void EntityInit(EcsEntity ecsEntity, EcsWorld ecsWorld, bool parentOnScene)
    {
        ecsEntity.Get<Hole>() = new Hole()
        {
            AttractingPoints = AttractingPoints,
            PullingFx = PullingFx,
            TubeFx = TubeFx,
            FirstPointRadius = FirstPointRadius,
        };
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer != 3)
            return;

        EntityRef entityRef = null;

        entityRef = other.transform.GetComponentInParent<EntityRef>();

        if (entityRef != null)
        {
            entityRef.Entity.Get<HoleTouched>();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(AttractingPoints == null || AttractingPoints.Length == 0)
            return;
        
        Color color = Color.green;
        color.a = 0.4f;
        Gizmos.color = color;
        
        Gizmos.DrawSphere(AttractingPoints[0].position, FirstPointRadius);
        
        
    }
}
public struct Hole
{
    public Transform[] AttractingPoints;
    public int PointIndex;
    public bool Pulling;
    public ParticleSystem PullingFx;
    public ParticleSystem TubeFx;
    public float FirstPointRadius;
}