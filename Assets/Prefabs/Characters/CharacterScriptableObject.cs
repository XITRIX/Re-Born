using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName 
    = "Character/Character")]
public class CharacterScriptableObject : ScriptableObject
{
    public List<Sprite> tileset;
}
