using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterScript))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInput : MonoBehaviour
{
    public float characterWalkSpeed = 2;
    public float characterRunSpeed = 4;
    
    private CharacterScript _character;
    private readonly List<EntityScript> _objectsToInteract = new();
    
    // Start is called before the first frame update
    void Awake()
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
        if (!entity || string.IsNullOrEmpty(entity.entityId)) { return; }
        
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

        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        var isHorizontal = Math.Abs(horizontal) > Math.Abs(vertical);
        
        if (isHorizontal && horizontal < 0)
        {
            direction = Vector2.left;
        }
        else if (isHorizontal && horizontal > 0)
        {
            direction = Vector2.right;
        }
        else if (!isHorizontal && vertical > 0)
        {
            direction = Vector2.up;
        }
        else if (!isHorizontal && vertical < 0)
        {
            direction = Vector2.down;
        }
        else
        {
            direction = Vector2.zero;
        }

        // _character.movementSpeed = Input.GetKey(KeyCode.LeftShift) ? characterRunSpeed : characterWalkSpeed;
        _character.movementSpeed = Input.GetButton("Fire3") ? characterRunSpeed : characterWalkSpeed;

        _character.direction = direction;
    }

    private void PerformInteraction() 
    {
        if (!Input.GetButtonDown("Submit") || _objectsToInteract.Count <= 0) return;

        var obj = _objectsToInteract.LastOrDefault();
        if (obj == null || obj.InteractionScenario == null) return;
        
        obj.InteractionScenario();
        
        Debug.Log($"Interact with {obj.entityId}");
    }
    
}
