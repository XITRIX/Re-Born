using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveCharacterUnit : Unit
{
    [DoNotSerialize]
    public ValueInput Id { get; private set; }
    
    [DoNotSerialize]
    public ValueInput Direction { get; private set; }
    
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlInput Enter { get; private set; }
 
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlOutput Exit { get; private set; }
    
    protected override void Definition()
    {
        Enter = ControlInputCoroutine("Enter", RunCoroutine);
        Exit = ControlOutput("Exit");
        Id = ValueInput("Id", "");
        Direction = ValueInput<Vector2>("Direction", Vector2.zero);
    }
    
    private IEnumerator RunCoroutine(Flow flow)
    {
        var idValue = flow.GetValue<string>(Id);
        var directionValue = flow.GetValue<Vector2>(Direction);

        var obj = GlobalDirector.Shared.GameObjectsStash[idValue];
        Debug.Log(idValue);
        var character = obj.GetComponent<CharacterScript>();

        var transform = character.transform;
        var startPos = transform.position;
        var targetPosition = startPos + (Vector3)directionValue;
        character.direction = directionValue.normalized;
        
        yield return new WaitWhile(() => Vector2.Distance(transform.position, targetPosition) > 0.1);
        transform.position = targetPosition;
        yield return Exit;
    }
}
