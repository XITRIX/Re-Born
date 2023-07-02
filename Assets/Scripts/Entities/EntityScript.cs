using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EntityScript : MonoBehaviour
{
    public string entityId;

    public void OnDestroy()
    {
        if (GlobalDirector.Shared.GameObjectsStash.ContainsKey(entityId) &&
            GlobalDirector.Shared.GameObjectsStash[entityId] == gameObject)
        {
            GlobalDirector.Shared.GameObjectsStash[entityId] = null;
            Debug.Log($"{entityId} destroyed");
        }
    }

    [CanBeNull]
    public Action InteractionScenario => () =>
    {
        EventBus.Trigger($"InteractionEvent", entityId);
    };
}
