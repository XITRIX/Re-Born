using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VNCanvasController : MonoBehaviour
{
    public static VNCanvasController Shared { get; private set; }

    public Image avatarField;
    public TextMeshProUGUI nameField;
    public TypewriterEffect messageField;

    // Start is called before the first frame update
    void Awake()
    {
        Shared = this;
    }
    
    public static void SetAvatar(Sprite avatar)
    {
        Shared.avatarField.enabled = avatar != null;
        Shared.avatarField.sprite = avatar;
    }

    public static void VNTest(VNDialogItemModel item)
    {
        
    }

    public static void SetName(string name)
    {
        Shared.nameField.SetText(name);
    }

    public static IEnumerator SetMessage(string message)
    {
        yield return Shared.messageField.SetText(message);
    }
}
