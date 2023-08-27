using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoPlayer : MonoBehaviour
{
    public float speed = 3;
    public float jumpForce = 10;
    public bool isOnFloor;

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
        if (DinoGame.shared.isDead)
        {
            var position = transform.position;
            position.x -= speed * Time.deltaTime;
            transform.position = position;
            
            _animator.enabled = false;
            
            if (Input.GetButtonDown("Jump")) {
                _rectTransform.anchoredPosition = new Vector2(-154, 0);
                _rectTransform.rotation = Quaternion.identity;
                DinoGame.shared.Restart();
            }
            
            return;
        }

        _animator.enabled = true;
        if (Input.GetButtonDown("Jump") && isOnFloor)
        {
            Debug.Log("JUMPED!");
            _rigidbody.AddForce(Vector2.up * jumpForce);
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Death"))
            DinoGame.shared.isDead = true;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
            isOnFloor = true;
        
        if (other.gameObject.CompareTag("Death"))
            DinoGame.shared.isDead = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
            isOnFloor = false;
    }
}
