using System;
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
    public Material originalMaterial;
    public Material translateMaterial;

    private Animator _animator;
    private CharacterController _characterController;
    private float _animationParamSpeed = 0.0f;
    private Vector3 _gravityVelocity = Vector3.zero;
    private bool _isGround = false;
    private FloatingJoystick _moveJoystick;

    private void Awake()
    {
        if (cameraTransform == null)
            FindCameraTransform();
        _animator = gameObject.GetComponent<Animator>();
        _characterController = gameObject.GetComponent<CharacterController>();
        
        // find joystick
        if (_moveJoystick == null)
            FindJoystick();
    }

    // Update is called once per frame
    void Update()
    {
        // find joystick
        if (_moveJoystick == null)
            FindJoystick();
        if (cameraTransform == null)
            FindCameraTransform();
        else
            PlayerMovement();
    }
    
    
    private void PlayerMovement()
    {
        // gravity control
        _gravityVelocity.y += gravity * Time.deltaTime;
        _characterController.Move(_gravityVelocity * Time.deltaTime);
        _isGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (_isGround && _gravityVelocity.y < 0)
            _gravityVelocity.y = -2f;

        // facing camera forward direction
        Vector3 facing = (transform.position - cameraTransform.position).normalized;
        facing.y = 0;
        Vector3 move = Vector3.zero;

        if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor)
        {
            // Windows Control
            if (Input.GetKey(KeyCode.W))
                move += facing;
            if (Input.GetKey(KeyCode.A))
                move += new Vector3(-facing.z, 0, facing.x);
            if (Input.GetKey(KeyCode.S))
                move += (-facing);
            if (Input.GetKey(KeyCode.D))
                move += new Vector3(facing.z, 0, -facing.x);

            if (Input.GetKey(KeyCode.Space) && _isGround)
            {
                _gravityVelocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
            }
        }
        else
        {
            // Android Control
            if (_moveJoystick != null)
            {
                if (Mathf.Abs(_moveJoystick.Vertical) > 0.15f)
                    move += facing * _moveJoystick.Vertical;
                if (Mathf.Abs(_moveJoystick.Horizontal) > 0.15f)
                    move += (new Vector3(facing.z, 0, -facing.x) * _moveJoystick.Horizontal);
            }
        }

        if (move != Vector3.zero)
        {
            Quaternion facingRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                facingRotation,
                facingSpeed * Time.deltaTime);
            _animationParamSpeed += Time.deltaTime * speed;
            _characterController.Move(move * speed * Time.deltaTime);
        }
        else
        {
            _animationParamSpeed -= Time.deltaTime * speed;
        }
        _animationParamSpeed = Mathf.Clamp(_animationParamSpeed, 0.0f, 0.5f);
        _animator.SetFloat("Speed", _animationParamSpeed);
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }

    private void FindCameraTransform()
    {
        Camera myCamera = Camera.main;
        if (myCamera != null)
        {
            cameraTransform = myCamera.transform;
            int childCount = gameObject.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = gameObject.transform.GetChild(i);
                if (child.gameObject.name == "Target Look At")
                    myCamera.gameObject.GetComponent<CameraControl>().lookAtPlayer = child;
            }
        }
    }

    private void FindJoystick()
    {
        FloatingJoystick[] joysticks = FindObjectsOfType<FloatingJoystick>();
        foreach (FloatingJoystick joystick in joysticks)
            if (joystick.gameObject.name == "Move Joystick")
                _moveJoystick = joystick;
    }

    private Vector3 _recordRebornPoint;
    
    public void TranslateTo(Vector3 position)
    {
        _characterController.enabled = false;
        Vector3 temp = transform.position;
        _animator.SetBool("Translate", true);
        _recordRebornPoint = position;
    }

    public void TranslateEnd()
    {
        _animator.SetBool("Translate", false);
        transform.position = _recordRebornPoint;
        _characterController.enabled = true;
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = originalMaterial;
    }
}
