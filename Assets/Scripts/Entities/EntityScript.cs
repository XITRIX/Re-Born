using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EntityScript : MonoBehaviour
{
    public string entityId;

    [CanBeNull]
    public Action InteractionScenario => () =>
    {
        EventBus.Trigger($"InteractionEvent", entityId);
    };
}
