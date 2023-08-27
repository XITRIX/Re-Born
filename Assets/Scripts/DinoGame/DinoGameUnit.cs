using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DinoGameUnit : Unit
{
    [DoNotSerialize]
    public ValueInput MaxScore { get; private set; }
    
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlInput Enter { get; private set; }
 
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlOutput Exit { get; private set; }
    
    protected override void Definition()
    {
        MaxScore = ValueInput<float>("MaxScore", 0);
        Enter = ControlInputCoroutine("Enter", RunCoroutine);
        Exit = ControlOutput("Exit");
    }
    
    private IEnumerator RunCoroutine(Flow flow)
    {
        var maxScore = flow.GetValue<float>(MaxScore);
        
        GlobalDirector.SetPlayerInputEnabled(false);
        GlobalDirector.ShowDinoGame(true);
        yield return new WaitUntil(() => DinoGame.shared.totalScore > maxScore);
        GlobalDirector.ShowDinoGame(false);
        GlobalDirector.SetPlayerInputEnabled(true);
        yield return Exit;
    }
}
