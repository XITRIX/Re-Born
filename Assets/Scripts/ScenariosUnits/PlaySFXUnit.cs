using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaySFXUnit : Unit
{
    [DoNotSerialize]
    public ValueInput Music { get; private set; }
    
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
        Music = ValueInput<AudioClip>("Music", null);
    }
    
    private IEnumerator RunCoroutine(Flow flow)
    {
        var music = flow.GetValue<AudioClip>(Music);

        // you might have to add some more logic to
        // detect if the character is destroyed or any special cases
        GlobalDirector.PlaySFX(music);
        yield return new WaitForSeconds(music.length);
        yield return Exit;
    }
}
