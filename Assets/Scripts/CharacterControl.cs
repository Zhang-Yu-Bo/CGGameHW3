﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float speed = 0;
    public float maxSpeed = 0.5f;
    public float minSpeed = 0.0f;

    private Animator _animator;
    private CharacterController _characterController;
    private Transform _playerTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _characterController = gameObject.GetComponent<CharacterController>();
        _playerTransform = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            speed += Time.deltaTime;
        }
        else
        {
            speed -= Time.deltaTime;
        }

        speed = Mathf.Clamp(speed, 0.0f, 0.5f);

        _animator.SetFloat("Speed", speed);
        Vector3 temp = _playerTransform.forward * speed;
        _characterController.Move(temp);
    }
}
