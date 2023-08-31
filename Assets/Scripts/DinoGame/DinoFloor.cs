using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoFloor : MonoBehaviour
{
    public float speed = 3;
    public float spawnCopyAfter = -138.2f;
    public float spawnCopyAt = 483.4f;
    public float destroyAt = -650;
    public bool randomY = false;
    public float yFrom = 0;
    public float yTo = 0;
    public float randomXRange = 0;
    
    private bool alreadySpawned = false;
    private RectTransform _rectTransform;
    private float randomRange;
    
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        randomRange = Random.Range(0, randomXRange);
    }

    // Update is called once per frame
    void Update()
    {
        var position = transform.position;
        position.x -= speed * Time.deltaTime;
        transform.position = position;
        
        Debug.Log(_rectTransform.anchoredPosition.x);

        if (!alreadySpawned && _rectTransform.anchoredPosition.x < spawnCopyAfter + randomRange)
        {
            var obj = Instantiate(gameObject, transform.parent);

            var y = _rectTransform.anchoredPosition.y;
            if (randomY)
            {
                y = Random.Range(yFrom, yTo);
            }

            ((RectTransform)obj.transform).anchoredPosition =
                new Vector2(spawnCopyAt, y);
            alreadySpawned = true;
        }

        if (_rectTransform.anchoredPosition.x < destroyAt)
        {
            Destroy(gameObject);
        }
    }
}
