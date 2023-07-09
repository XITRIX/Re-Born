using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Scenario Units/Fade screen")]
[UnitSubtitle("Fade screen")]
public class FadeBackgroundUnit : Unit
{
    [DoNotSerialize]
    public ValueInput Fade { get; private set; }
    
    [DoNotSerialize]
    public ValueInput Seconds { get; private set; }
    
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
        Fade = ValueInput<bool>("Fade", false);
        Seconds = ValueInput<float>("Seconds", 0);
    }
    
    private IEnumerator RunCoroutine(Flow flow)
    {
        var fadeValue = flow.GetValue<bool>(Fade);
        var secondsValue = flow.GetValue<float>(Seconds);

        yield return VNCanvasController.Shared.ChangeBackgroundFading(fadeValue, secondsValue);
        yield return Exit;
    }
}
