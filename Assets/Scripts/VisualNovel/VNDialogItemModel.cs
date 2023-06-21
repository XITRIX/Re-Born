using System;
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
}
