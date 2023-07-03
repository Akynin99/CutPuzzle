using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.UserInput;
using Modules.ViewHub;
using Obi;
using UnityEngine;

public class RopeInitSystem : IEcsRunSystem
{
    private EcsFilter<RopeView> _filter;

    private EcsWorld _world;

    public void Run()
    {
        foreach (var i in _filter)
        {
            if(_filter.Get1(i).Inited)
                continue;

            _filter.Get1(i).Inited = true;

            ObiRope rope = _filter.Get1(i).View;
            ObiParticleAttachment[] attachments = rope.GetComponents<ObiParticleAttachment>();
            Transform target = null;

            foreach (var attachment in attachments)
            {
                if(attachment.attachmentType != ObiParticleAttachment.AttachmentType.Dynamic)
                    continue;
                
                if(attachment.target.GetComponent<Rigidbody>() == null)
                    continue;

                target = attachment.target;
                
                break;
            }
            
            if(target == null)
                continue;

            EntityRef entityRef = null;

            entityRef = target.GetComponentInParent<EntityRef>();
            
            if(entityRef == null)
                continue;

            _filter.Get1(i).AttachedEntity = entityRef.Entity;
        }
    }
}


