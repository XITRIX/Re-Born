using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VNMessageUnit : Unit
{
    [DoNotSerialize]
    public ValueInput Avatar { get; private set; }
    
    [DoNotSerialize]
    public ValueInput Name { get; private set; }
    
    [DoNotSerialize]
    public ValueInput Message { get; private set; }
    
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
        Avatar = ValueInput<Sprite>("Avatar", null);
        Name = ValueInput<string>("Name", "");
        Message = ValueInput<string>("Message", "");
    }
    
    private IEnumerator RunCoroutine(Flow flow)
    {
        var avatar = flow.GetValue<Sprite>(Avatar);
        var name = flow.GetValue<string>(Name);
        var message = flow.GetValue<string>(Message);
        VNCanvasController.ShowMessageCanvas(true);
        VNCanvasController.SetAvatar(avatar);
        VNCanvasController.SetName(name);
        yield return VNCanvasController.SetMessage(message);
        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
        VNCanvasController.ShowMessageCanvas(false);
        yield return Exit;
    }
}
