using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterScript : EntityScript
{
    enum Direction
    {
        Down, Left, Right, Up
    }

    public float movementSpeed = 1;
    public int animationTick = 10;
    public CharacterScriptableObject characterModel;
    
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;

    public Vector2 direction;
    private Direction _lastDirection;
    
    private int _animationCounter;
    private int _animationFrame;
    
    // Start is called before the first frame update
    public void Awake()
    {
        GlobalDirector.Shared.GameObjectsStash[entityId] = gameObject;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        PerformAnimation();
    }

    private void Start()
    {
        if (GlobalDirector.Shared.currentPlayerObject == null || GlobalDirector.Shared.currentPlayerObject.gameObject != gameObject)
        {
            GetComponent<Rigidbody2D>().mass = 99999;
        }
        else
        {
            _spriteRenderer.sortingOrder = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        PerformMovement();
        PerformAnimation();
    }

    private void PerformMovement()
    {
        Vector2 velocity;
        var speed = movementSpeed;
        
        if (direction.x > 0)
        {
            velocity = Vector2.right * speed;
        }
        else if (direction.x < 0)
        {
            velocity = Vector2.left * speed;
        }
        else if (direction.y > 0)
        {
            velocity = Vector2.up * speed;
        }
        else if (direction.y < 0)
        {
            velocity = Vector2.down * speed;
        }
        else
        {
            velocity = Vector2.zero;
        }

        _rigidbody2D.velocity = velocity;
    }

    private void PerformAnimation()
    {
        var isIdle = false;
        
        if (direction.x > 0)
        {
            _lastDirection = Direction.Right;
        }
        else if (direction.x < 0)
        {
            _lastDirection = Direction.Left;
        }
        else if (direction.y > 0)
        {
            _lastDirection = Direction.Up;
        }
        else if (direction.y < 0)
        {
            _lastDirection = Direction.Down;
        }
        else
        {
            _animationCounter = 0;
            _animationFrame = 0;
            isIdle = true;
        }
        
        _spriteRenderer.sprite = FrameForDirection(_lastDirection, _animationFrame, isIdle);

        _animationCounter += (int)movementSpeed / 2;
        if (_animationCounter >= animationTick)
        {
            _animationCounter = 0;
            _animationFrame++;
            _animationFrame %= 4;
        }
    }

    private Sprite FrameForDirection(Direction direction, int frame, bool isIdle)
    {
        var localFrame = !isIdle ? frame : 1;
        
        int[] animationSequence = { 2, 1, 0, 1 };

        return direction switch
        {
            Direction.Down => characterModel.tileset[animationSequence[localFrame]],
            Direction.Left => characterModel.tileset[3 + animationSequence[localFrame]],
            Direction.Right => characterModel.tileset[6 + animationSequence[localFrame]],
            Direction.Up => characterModel.tileset[9 + animationSequence[localFrame]],
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}
