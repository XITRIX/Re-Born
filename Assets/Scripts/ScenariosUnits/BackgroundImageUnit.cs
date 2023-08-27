using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Scenario Units/Image screen")]
[UnitSubtitle("Image screen")]
public class BackgroundImageUnit : Unit
{
    [DoNotSerialize]
    public ValueInput Image { get; private set; }
    
    [DoNotSerialize]
    public ValueInput Fit { get; private set; }
    
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
        Image = ValueInput<Sprite>("Image", null);
        Fit = ValueInput<bool>("Fit", false);
        Seconds = ValueInput<float>("Seconds", 0);
    }
    
    private IEnumerator RunCoroutine(Flow flow)
    {
        var image = flow.GetValue<Sprite>(Image);
        var fit = flow.GetValue<bool>(Fit);
        var secondsValue = flow.GetValue<float>(Seconds);

        GlobalDirector.SetPlayerInputEnabled(false);
        yield return VNCanvasController.Shared.SetBackgroundImage(image, fit, secondsValue);
        GlobalDirector.SetPlayerInputEnabled(true);
        yield return Exit;
    }
}
