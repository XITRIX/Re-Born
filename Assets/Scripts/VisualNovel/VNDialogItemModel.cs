using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Inspectable]
public class VNDialogItemModel
{
    [Inspectable]
    public Sprite avatar;
    
    [Inspectable]
    public string name;

    [Inspectable] 
    public string message;

    public VNDialogItemModel(string name, string message, Sprite avatar)
    {
        this.name = name;
        this.message = message;
        this.avatar = avatar;
    }

    public VNDialogItemModel(string id, string message)
    {
        var obj = GlobalDirector.Shared.characters.First(x => x.id == id).gameObject;
        name = obj.name;
        avatar = obj.avatar;
        this.message = message;
    }
}
