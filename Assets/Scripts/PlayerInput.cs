using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterScript))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInput : MonoBehaviour
{
    private CharacterScript _character;
    private readonly List<EntityScript> _objectsToInteract = new();
    
    // Start is called before the first frame update
    void Start()
    {
        _character = GetComponent<CharacterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        PerformMovement();
        PerformInteraction();
    }

    public void ForceStop()
    {
        _character.direction = Vector2.zero;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        var entity = col.GetComponent<EntityScript>();
        if (!entity || entity.entityId == null) { return; }
        
        // Debug.Log($"Enter: {entity.entityId}");

        _objectsToInteract.Add(entity);
        EventBus.Trigger($"InteractionTriggerEnterUnit", entity.entityId);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        var entity = col.GetComponent<EntityScript>();
        if (!entity || entity.entityId == null) { return; }
        
        // Debug.Log($"Exit: {entity.entityId}");
        
        _objectsToInteract.Remove(entity);
        EventBus.Trigger($"InteractionTriggerExitUnit", entity.entityId);
    }

    private void PerformMovement()
    {
        Vector2 direction;
            
        if (Input.GetKey(KeyCode.A))
        {
            direction = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = Vector2.down;
        }
        else
        {
            direction = Vector2.zero;
        }

        _character.direction = direction;
    }

    private void PerformInteraction()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || _objectsToInteract.Count <= 0) return;

        var obj = _objectsToInteract.LastOrDefault();
        if (obj == null || obj.InteractionScenario == null) return;
        
        obj.InteractionScenario();
        
        // Debug.Log($"Interact with {obj.entityId}");
    }
    
}
