using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Scenario Units/Load Character")]
public class LoadCharacterUnit : ManualEventUnit<Unit>
{
    [DoNotSerialize] 
    public ValueInput IsPlayer { get; private set; }

    [DoNotSerialize] 
    public ValueInput Id { get; private set; }
    
    // [DoNotSerialize] 
    public ValueInput Position { get; private set; }
    
    // [DoNotSerialize] 
    public ValueInput Character { get; private set; }

    [DoNotSerialize] 
    [PortLabelHidden] 
    public ControlInput Enter { get; private set; }

    [DoNotSerialize] 
    [PortLabelHidden] 
    public ControlOutput Exit { get; private set; }

    protected override void Definition()
    {
        IsPlayer = ValueInput("IsPlayer", false);
        Id = ValueInput("Id", "");
        Position = ValueInput<Vector2>("Position");
        Character = ValueInput<CharacterScriptableObject>("Character", null);
        Enter = ControlInput("Enter", Run);
        Exit = ControlOutput("Exit");
    }

    public override void StartListening(GraphStack stack)
    {
        base.StartListening(stack);
        
    }

    private ControlOutput Run(Flow flow)
    {
        var isPlayer = flow.GetValue<bool>(IsPlayer);
        var id = flow.GetValue<string>(Id);
        var position = flow.GetValue<Vector2>(Position);
        var character = flow.GetValue<CharacterScriptableObject>(Character);

        GlobalDirector.LoadCharacter(id, character, position, isPlayer);

        return Exit;
    }

    protected override string hookName => "Load Character";
}
