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
        _textBox.text = "";

        var trimmedString = originalString.Trim();
        var numCharsRevealed = 0;
        while (numCharsRevealed < trimmedString.Length)
        {
            startSkipping |= !Input.GetKey(KeyCode.Space);
            
            while (originalString[numCharsRevealed] == ' ')
                ++numCharsRevealed;

            ++numCharsRevealed;

            var newText = trimmedString.Insert(numCharsRevealed, "<color=#00000000>") + "</color>";
            _textBox.text = newText;

            yield return new WaitForSeconds(startSkipping && Input.GetKey(KeyCode.Space) ? 0.01f : 0.07f);
        }
    } 
}
