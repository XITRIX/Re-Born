using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DinoGame : MonoBehaviour
{
    public static DinoGame shared;
    public Canvas canvas;
    public TextMeshProUGUI text;

    public float totalScore = 0;
    public float score = 0;
    public float speed = 1;
    public bool isDead = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        shared = this;
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.sortingLayerName = "Foreground";
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            text.text = "DEAD\n\nJump to suffer more";
        }
        else
        {
            totalScore += Time.deltaTime;
            score += Time.deltaTime;
            text.text = ((int)score).ToString();
        }
    }

    public void Restart()
    {
        score = 0;
        isDead = false;
    }
}
