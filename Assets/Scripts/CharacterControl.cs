using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float speed;
    public float facingSpeed;
    public float gravity = -9.81f;
    public Transform cameraTransform;
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    public float jumpPower;

    private Animator _animator;
    private CharacterController _characterController;
    private float _animaParamSpeed = 0.0f;
    private Vector3 _gravityVelocity = Vector3.zero;
    private bool isGround = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {

        if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor)
        {
            _gravityVelocity.y += gravity * Time.deltaTime;
            _characterController.Move(_gravityVelocity * Time.deltaTime);
            isGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGround && _gravityVelocity.y < 0)
                _gravityVelocity.y = -2f;

            Vector3 facing = (transform.position - cameraTransform.position).normalized;
            facing.y = 0;
            Vector3 move = Vector3.zero;
            
            if (Input.GetKey(KeyCode.W))
                move += facing;
            if (Input.GetKey(KeyCode.A))
                move += new Vector3(-facing.z, 0, facing.x);
            if (Input.GetKey(KeyCode.S))
                move += (-facing);
            if (Input.GetKey(KeyCode.D))
                move += new Vector3(facing.z, 0, -facing.x);
            
            if (move != Vector3.zero)
            {
                Quaternion facingRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, facingRotation, facingSpeed * Time.deltaTime);
                _animaParamSpeed += Time.deltaTime * speed;
                _characterController.Move(move * speed * Time.deltaTime);
            }
            else
            {
                _animaParamSpeed -= Time.deltaTime * speed;
            }

            if (Input.GetKey(KeyCode.Space) && isGround)
            {
                _gravityVelocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
            }

            _animaParamSpeed = Mathf.Clamp(_animaParamSpeed, 0.0f, 0.5f);
            _animator.SetFloat("Speed", _animaParamSpeed);
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
