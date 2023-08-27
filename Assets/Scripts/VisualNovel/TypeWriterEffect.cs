using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Object = UnityEngine.Object;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterEffect : MonoBehaviour
{
    private TextMeshProUGUI _textBox;
    private bool startSkipping = false;

    private void Awake()
    {
        _textBox = GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator SetText(string text)
    {
        yield return StartCoroutine(RevealText(text));
    }
    
    IEnumerator RevealText(string originalString)
    {
        startSkipping = false;
        // _textBox.text = "";

        var trimmedString = originalString.Trim();
        _textBox.text = trimmedString;
        _textBox.maxVisibleCharacters = 0;
        
        var numCharsRevealed = 0;
        
        VNCanvasController.Shared.audioSource.Play();

        // var count = _textBox.textInfo.characterCount > 0 ? _textBox.textInfo.characterCount : trimmedString.Length;
        _textBox.ForceMeshUpdate();
        while (numCharsRevealed < _textBox.textInfo.characterCount)
        // while (numCharsRevealed < trimmedString.Length)
        {
            startSkipping |= !Input.GetButton("Submit");
            
            while (numCharsRevealed < originalString.Length && originalString[numCharsRevealed] == ' ')
                ++numCharsRevealed;

            // if (originalString[numCharsRevealed] == '<')
            // {
            //     do ++numCharsRevealed;
            //     while (originalString[numCharsRevealed] != '>');
            // }
            
            ++numCharsRevealed;
            _textBox.maxVisibleCharacters = numCharsRevealed;

            yield return new WaitForSeconds(startSkipping && Input.GetButton("Submit") ? 0.01f : 0.07f);
        }
        VNCanvasController.Shared.audioSource.Stop();
    } 
}
