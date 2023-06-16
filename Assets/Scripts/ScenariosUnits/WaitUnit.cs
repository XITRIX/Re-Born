using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Scenario Units/Wait")]
[UnitSubtitle("Wait for seconds")]
public class WaitUnit : Unit
{
    [DoNotSerialize]
    public ValueInput Time { get; private set; }
    
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
        Time = ValueInput<float>("Time", 0);
    }
    
    private IEnumerator RunCoroutine(Flow flow)
    {
        var timeValue = flow.GetValue<float>(Time);

        // you might have to add some more logic to
        // detect if the character is destroyed or any special cases
        yield return new WaitForSeconds(timeValue);
        yield return Exit;
    }

}
