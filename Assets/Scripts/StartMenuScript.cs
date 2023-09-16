using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public bool isStarting = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStarting)
        {
            var sin = (Mathf.Sin(Time.time) + 1) / 2;
            var alpha = sin * 0.8f + 0.2f;
            text.alpha = alpha;
        }

        if (!Input.anyKeyDown || isStarting) return;
        
        isStarting = true;
        StartCoroutine(StartGame(2));
    }

    private IEnumerator StartGame(float duration)
    {
        float elapsedTime = 0;
        var startValue = text.alpha;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            var newAlpha = Mathf.Lerp(startValue, 0, elapsedTime / duration);
            text.alpha = newAlpha;
            yield return null;
        }
        SceneManager.LoadScene(1);
    }

}
