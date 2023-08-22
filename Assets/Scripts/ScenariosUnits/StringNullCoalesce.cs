using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StringNullCoalesce : Unit
{
    [DoNotSerialize]
    public ValueInput Input { get; private set; }
    
    [DoNotSerialize]
    public ValueInput Fallback { get; private set; }

    [DoNotSerialize] 
    public ValueOutput Output { get; private set; }

    protected override void Definition()
    {
        Input = ValueInput<string>("Input", "");
        Fallback = ValueInput<string>("Fallback", "");
        Output = ValueOutput<string>("Output", delegate(Flow flow)
        {
            var inputValue = flow.GetValue<string>(Input);
            var fallbackValue = flow.GetValue<string>(Fallback);
            return string.IsNullOrEmpty(inputValue) ? fallbackValue : inputValue;
        });
    }
}
