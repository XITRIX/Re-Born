using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VNDialogChoiseUnit : Unit, IBranchUnit
{
    // Using L<KVP> instead of Dictionary to allow null key
    [DoNotSerialize]
    public List<KeyValuePair<string, ControlOutput>> branches { get; private set; }

    [Inspectable, Serialize]
    public List<string> options { get; set; } = new List<string>();
    
    [DoNotSerialize]
    [PortLabelHidden]
    [AllowsNull]
    public ValueInput Excludes { get; private set; }
    
    /// <summary>
    /// The entry point for the switch.
    /// </summary>
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlInput enter { get; private set; }
    
    protected override void Definition()
    {
        enter = ControlInputCoroutine(nameof(enter), Enter);
        Excludes = ValueInput<List<string>>("Excludes");

        branches = new List<KeyValuePair<string, ControlOutput>>();

        foreach (var option in options)
        {
            var key = "%" + option;

            if (!controlOutputs.Contains(key))
            {
                var branch = ControlOutput(key);
                branches.Add(new KeyValuePair<string, ControlOutput>(option, branch));
                Succession(enter, branch);
            }
        }

        // @default = ControlOutput(nameof(@default));
        // Succession(enter, @default);
    }

    private IEnumerator Enter(Flow flow)
    {
        var excludesValue = new List<string>();
        try
        {
            excludesValue = flow.GetValue<List<string>>(Excludes);
        } catch { /* ignored */ }

        GlobalDirector.SetPlayerInputEnabled(false);
        VNCanvasController.ShowMessageCanvas(true);
        VNCanvasController.Shared.buttonsHolder.enabled = true;

        var selected = "";
        foreach (var branch in branches)
        {
            if (excludesValue.Contains(branch.Key)) continue;
            
            var button = Object.Instantiate(VNCanvasController.Shared.buttonPrefab, VNCanvasController.Shared.buttonsHolder.transform, true);
            button.GetComponentInChildren<TextMeshProUGUI>().text = branch.Key;
            button.onClick.AddListener(() =>
            {
                selected = branch.Key;
            });
        }

        yield return new WaitUntil(() => !string.IsNullOrEmpty(selected));
        VNCanvasController.ShowMessageCanvas(false);
        
        foreach (Transform child in VNCanvasController.Shared.buttonsHolder.transform) {
            Object.Destroy(child.gameObject);
        }
        
        VNCanvasController.Shared.buttonsHolder.enabled = false;
        GlobalDirector.SetPlayerInputEnabled(true);
        
        yield return branches.FirstOrDefault(x => x.Key == selected).Value;
    }
}
