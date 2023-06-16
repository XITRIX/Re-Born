using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScript : MonoBehaviour
{
    public static CameraScript Shared { get; private set; }

    public GameObject followedObject;

    private Transform Transform { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        Shared = this;
    }

    private void Start()
    {
        Transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        if (!followedObject) return;
        
        var position = Transform.position;

        position.x = followedObject.transform.position.x;
        position.y = followedObject.transform.position.y;

        Transform.position = position;
    }
}
