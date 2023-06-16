using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Object = UnityEngine.Object;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterEffect : MonoBehaviour
{
    private TextMeshProUGUI _textBox;

    private void Awake()
    {
        _textBox = GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator SetText(string text)
    {
        yield return StartCoroutine(RevealText(text));
    }
    
    IEnumerator RevealText(string originalString) {
        _textBox.text = "";

        var numCharsRevealed = 0;
        while (numCharsRevealed < originalString.Length)
        {
            while (originalString[numCharsRevealed] == ' ')
                ++numCharsRevealed;

            ++numCharsRevealed;

            var newText = originalString.Insert(numCharsRevealed, "<color=#00000000>") + "</color>";
            _textBox.text = newText;

            yield return new WaitForSeconds(0.07f);
        }
    } 
}
