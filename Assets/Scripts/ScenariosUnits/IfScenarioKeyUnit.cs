using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Scenario Units/If Scenario key")]
[UnitSubtitle("If Scenario key presented")]
public class IfScenarioKeyUnit : Unit
{
    [DoNotSerialize]
    public ValueInput Key { get; private set; }
    
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlInput Enter { get; private set; }
 
    [DoNotSerialize]
    public ControlOutput True { get; private set; }
    
    [DoNotSerialize]
    public ControlOutput False { get; private set; }
    
    protected override void Definition()
    {
        Enter = ControlInput("Enter", Run);
        True = ControlOutput("True");
        False = ControlOutput("False");
        Key = ValueInput<string>("Key", "");
    }
    
    private ControlOutput Run(Flow flow)
    {
        var keyValue = flow.GetValue<string>(Key);
        return GlobalDirector.GetScenarioKey(keyValue) ? True : False;
    }
}
