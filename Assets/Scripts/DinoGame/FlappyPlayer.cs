using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyPlayer : MonoBehaviour
{
    public float speed = 3;
    public float jumpForce = 10;

    private RectTransform _rectTransform;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FlappyGame.shared.isDead)
        {
            var position = transform.position;
            position.x -= speed * Time.deltaTime;
            transform.position = position;
            
            _animator.enabled = false;
            
            if (Input.GetButtonDown("Jump")) {
                _rectTransform.anchoredPosition = new Vector2(-154, 80);
                _rectTransform.rotation = Quaternion.identity;
                FlappyGame.shared.Restart();
            }
            
            return;
        }

        _animator.enabled = true;
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("JUMPED!");
            _rigidbody.AddForce(Vector2.up * jumpForce);
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Death"))
            FlappyGame.shared.isDead = true;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Death"))
            FlappyGame.shared.isDead = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Score") || FlappyGame.shared.isDead) return;

        FlappyGame.shared.totalScore += 1;
        FlappyGame.shared.score += 1;
    }
}
