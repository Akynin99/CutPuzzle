using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.Utils;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class BombDetonateSystem : IEcsRunSystem
{
    private EcsFilter<Bomb, BombDetonate, UnityView> _filter;
    private EcsFilter<Human, UnityView>.Exclude<InFight> _humans;
    private EcsFilter<Enemy, UnityView>.Exclude<InFight> _enemies;
    private EcsFilter<Spikes, UnityView> _spikes;
    private EcsFilter<Ball, UnityView> _balls;
    private EcsFilter<RopeView> _ropes;

    private EcsWorld _world;

    public void Run()
    {
        foreach (var bombIdx in _filter)
        {
            ref var bomb = ref _filter.Get1(bombIdx);
            
            if(bomb.ExplosionFx)
                bomb.ExplosionFx.Play();
            
            if(bomb.Visuals)
                bomb.Visuals.SetActive(false);

            // bomb.Rigidbody.useGravity = false;
            // bomb.Rigidbody.isKinematic = true;
            // bomb.Collider.enabled = false;

            if (bomb.Collider is SphereCollider sphereCollider)
            {
                sphereCollider.radius = 0.01f;
            }
            else if (bomb.Collider is BoxCollider boxCollider)
            {
                boxCollider.size = Vector3.one * 0.01f;
            }

            _filter.Get3(bombIdx).GameObject.layer = LayerMask.NameToLayer("Default");

            // foreach (var ropeIdx in _ropes)
            // {
            //     if(_ropes.Get1(ropeIdx).AttachedEntity.IsNull())
            //         continue;
            //     if (!_ropes.Get1(ropeIdx).AttachedEntity.Has<Bomb>())
            //         continue;
            //     if (!_ropes.Get1(ropeIdx).AttachedEntity.Equals(_filter.GetEntity(bombIdx)))
            //         continue;
            //
            //     ref var rope = ref _ropes.Get1(ropeIdx);
            //     ObiParticleAttachment[] attachments = rope.View.GetComponents<ObiParticleAttachment>();
            //
            //     foreach (var attachment in attachments)
            //     {
            //         if (attachment.attachmentType == ObiParticleAttachment.AttachmentType.Dynamic)
            //             attachment.target = bomb.ObjectForRope;
            //     }
            // }
            //
            // bomb.ObjectForRope.gameObject.SetActive(true);

            Explosion(_filter.Get3(bombIdx).Transform.position, _filter.Get1(bombIdx).Range,
                _filter.Get1(bombIdx).ExplosionForce, bomb.PushBall, bomb.DestroyRopes);

            _filter.GetEntity(bombIdx).Del<Bomb>();
            _filter.GetEntity(bombIdx).Del<BombDetonate>();
        }
    }

    private void Explosion(Vector3 center, float radius, float force, bool pushBalls, bool destroyRopes)
    {
        if(pushBalls)
            foreach (var ballIdx in _balls)
            {
                _balls.Get1(ballIdx).Rigidbody.AddExplosionForce(force, center, radius);
            }
        
        
        foreach (var humanIdx in _humans)
        {
            bool death = false;

            ref var human = ref _humans.Get1(humanIdx);

            for (int i = 0; i < human.HitPoints.Length; i++)
            {
                if(Vector3.Distance(center, human.HitPoints[i].position) > radius)
                    continue;

                death = true;
                break;
            }
            
            if(!death)
                continue;
            
            _humans.GetEntity(humanIdx).Get<DeathTag>();

            if (!_humans.GetEntity(humanIdx).Has<RagdollComponent>())
                continue;

            var ragdoll = _humans.GetEntity(humanIdx).Get<RagdollComponent>();

            foreach (var rigidbody in ragdoll.Rigidbodies) rigidbody.AddExplosionForce(force, center, radius);
        }
        
        foreach (var enemyIdx in _enemies)
        {
            bool death = false;

            ref var enemy = ref _enemies.Get1(enemyIdx);

            for (int i = 0; i < enemy.HitPoints.Length; i++)
            {
                if(Vector3.Distance(center, enemy.HitPoints[i].position) > radius)
                    continue;

                death = true;
                break;
            }
            
            if(!death)
                continue;
            
            _enemies.GetEntity(enemyIdx).Get<DeathTag>();

            if (!_enemies.GetEntity(enemyIdx).Has<RagdollComponent>())
                continue;

            var ragdoll = _enemies.GetEntity(enemyIdx).Get<RagdollComponent>();

            foreach (var rigidbody in ragdoll.Rigidbodies) rigidbody.AddExplosionForce(force, center, radius);
        }

        foreach (var spikeIdx in _spikes)
        {
            if (Vector3.Distance(center, _spikes.Get2(spikeIdx).Transform.position) > radius)
                continue;

            _spikes.Get1(spikeIdx).Visuals.SetActive(false);
            _spikes.Get1(spikeIdx).Collider.enabled = false;
            _spikes.GetEntity(spikeIdx).Del<Spikes>();
        }
        
        if(destroyRopes)
            foreach (var ropeIdx in _ropes)
            {
                ObiRope rope = _ropes.Get1(ropeIdx).View;

                if (rope.blueprint == null)
                    continue;
            

                for (var i = 1; i < rope.elements.Count - 1; i++)
                {
                    Vector3 particle1 = rope.GetParticlePosition(rope.elements[i].particle1);
                    Vector3 particle2 = rope.GetParticlePosition(rope.elements[i].particle2);
                    Vector3 elementPos = (particle1 + particle2) / 2;

                    if (Vector3.Distance(elementPos, center) <= radius)
                    {
                        rope.Tear(rope.elements[i]);
                        rope.RebuildConstraintsFromElements();
                    }
                }
            }
    }
    
}

public struct BombDetonate : IEcsIgnoreInFilter
{
    
}


