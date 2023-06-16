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

    [DoNotSerialize] 
    public ControlOutput Interaction { get; private set; }

    protected override void Definition()
    {
        IsPlayer = ValueInput("IsPlayer", false);
        Id = ValueInput("Id", "");
        Position = ValueInput(typeof(Vector2), "Position");
        Character = ValueInput(typeof(CharacterScriptableObject), "Character");
        Enter = ControlInput("Enter", Run);
        Exit = ControlOutput("Exit");
        Interaction = ControlOutput("Interaction");
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

        if (isPlayer && GlobalDirector.Shared.currentPlayerObject)
        {
            Debug.LogError("Player object is already presented");
            return Exit;
        }

        var obj = GameObject.Instantiate(GlobalDirector.Shared.prefabs.First(v => v.id == "CharPrefab").gameObject,
            position, Quaternion.identity);

        var characterObj = obj.GetComponent<CharacterScript>();
        characterObj.entityId = id;
        characterObj.characterModel = character;

        if (isPlayer)
        {
            GlobalDirector.Shared.currentPlayerObject = characterObj.AddComponent<PlayerInput>();
        }
        else
        {
            characterObj.GetComponent<Rigidbody2D>().mass = 99999;
        }

        GlobalDirector.Shared.GameObjectsStash[id] = characterObj.gameObject;

        return Exit;
    }

    protected override string hookName => "Load Character";
}
