using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Scenario Units/Move object to coordinate")]
[UnitSubtitle("Wait for seconds")]
public class MoveObjectToCoordinateUnit : Unit
{
    [DoNotSerialize]
    public ValueInput Id { get; private set; }
    
    [DoNotSerialize]
    public ValueInput Position { get; private set; }
    
    [DoNotSerialize]
    public ValueInput Wait { get; private set; }
    
    [DoNotSerialize]
    public ValueInput Duration { get; private set; }
    
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlInput Enter { get; private set; }
 
    [DoNotSerialize]
    public ControlOutput Exit { get; private set; }
    
    protected override void Definition()
    {
        Enter = ControlInputCoroutine("Enter", RunCoroutine);
        Exit = ControlOutput("Exit");
        Id = ValueInput("Id", "");
        Position = ValueInput("Position", Vector2.zero);
        Wait = ValueInput<bool>("Wait", true);
        Duration = ValueInput<float>("Duration", 0);
    }
    
    private IEnumerator RunCoroutine(Flow flow)
    {
        var id = flow.GetValue<string>(Id);
        var position = flow.GetValue<Vector2>(Position);
        var timeValue = flow.GetValue<float>(Duration);
        var wait = flow.GetValue<bool>(Wait);
        
        var obj = GlobalDirector.GetEntityById(id).GetComponent<EntityScript>();

        if (wait)
            yield return MoveObjectWait(obj.gameObject, position, timeValue);
        else
            obj.StartCoroutine(MoveObjectWait(obj.gameObject, position, timeValue));
        
        
        yield return Exit;
    }

    private IEnumerator MoveObjectWait(GameObject obj, Vector2 pos, float time)
    {
        var fromPos = obj.transform.position;
        float elapsedTime = 0;
        
        while (elapsedTime < time)
        {
            obj.transform.position = Vector3.Lerp(fromPos, pos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
