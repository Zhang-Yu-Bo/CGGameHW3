using System;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform lookAtPlayer;
    public float yDistance = 5;
    public float maxDistance = 10;
    public float minDistance = 1;
    public float xAngle = 270;
    public float yAngle = 37;
    public float maxYAngle = 60;
    public float minYAngle = -60;
    public float xAngleSensitivity = 10;
    public float yAngleSensitivity = 2;
    public float scaleSensitivity = 10;
    
    private FloatingJoystick _rotateJoystick;
    private FloatingJoystick _moveJoystick;
    private GameObject _joystickObjectRotate;
    private GameObject _joystickObjectMove;

    private void Awake()
    {
        if (lookAtPlayer != null)
            ControlViewport();
        if (_rotateJoystick == null)
        {
            FloatingJoystick[] joysticks = FindObjectsOfType<FloatingJoystick>();
            foreach (FloatingJoystick joystick in joysticks)
            {
                if (joystick.gameObject.name == "Rotate Joystick")
                {
                    _rotateJoystick = joystick;
                    _joystickObjectRotate = _rotateJoystick.transform.GetChild(0).gameObject;
                }else if (joystick.gameObject.name == "Move Joystick")
                {
                    _moveJoystick = joystick;
                    _joystickObjectMove = _moveJoystick.transform.GetChild(0).gameObject;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lookAtPlayer != null)
            ControlViewport();
        if (_rotateJoystick == null)
        {
            FloatingJoystick[] joysticks = FindObjectsOfType<FloatingJoystick>();
            foreach (FloatingJoystick joystick in joysticks)
            {
                if (joystick.gameObject.name == "Rotate Joystick")
                {
                    _rotateJoystick = joystick;
                    _joystickObjectRotate = _rotateJoystick.transform.GetChild(0).gameObject;
                }else if (joystick.gameObject.name == "Move Joystick")
                {
                    _moveJoystick = joystick;
                    _joystickObjectMove = _moveJoystick.transform.GetChild(0).gameObject;
                }
            }
        }
    }

    void ControlViewport()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor)
        {
            yDistance -= Input.GetAxis("Mouse ScrollWheel") * scaleSensitivity;
            if (Input.GetMouseButton(1))
            {
                Cursor.visible = false;
                xAngle -= Input.GetAxis("Mouse X") * xAngleSensitivity;
                yAngle -= Input.GetAxis("Mouse Y") * yAngleSensitivity;
            }
            else
            {
                Cursor.visible = true;
            }
            
            // Android Control
            if (_rotateJoystick != null)
            {
                // rotate viewport
                if (_rotateJoystick.Horizontal >= 0.5f || _rotateJoystick.Horizontal <= -0.5f)
                    xAngle -= (_rotateJoystick.Horizontal * xAngleSensitivity / 2);
                if (_rotateJoystick.Vertical >= 0.5f  || _rotateJoystick.Vertical <= -0.5f)
                    yAngle -= _rotateJoystick.Vertical;
                // scale
                if (Input.touchCount == 2 && 
                    !_joystickObjectRotate.activeSelf &&
                    !_joystickObjectMove.activeSelf)
                {
                    Vector2 finger1 = Input.touches[0].position;
                    Vector2 moveFinger1 = finger1 + Input.touches[0].deltaPosition;
                    Vector2 finger2 = Input.touches[1].position;
                    Vector2 moveFinger2 = finger2 + Input.touches[1].deltaPosition;

                    float fingerDistance = (finger1 - finger2).magnitude;
                    float moveFingerDistance = (moveFinger1 - moveFinger2).magnitude;
                    yDistance -= ((moveFingerDistance - fingerDistance) / (scaleSensitivity * 2));

                }
            }

        }else if (Application.platform == RuntimePlatform.Android)
        {
            // test
        }

        xAngle = xAngle > 360 ? 0 : xAngle;
        xAngle = xAngle < 0 ? 360 : xAngle;
        yAngle = Mathf.Clamp(yAngle, minYAngle, maxYAngle);
        yDistance = Mathf.Clamp(yDistance, minDistance, maxDistance);
        float xDistance = yDistance * Mathf.Cos(EulerToRadian(yAngle));
        
        Vector3 newPosition = lookAtPlayer.position +
                              Vector3.forward * xDistance * Mathf.Sin(EulerToRadian(xAngle)) +
                              Vector3.up * yDistance * Mathf.Sin(EulerToRadian(yAngle)) +
                              Vector3.right * xDistance * Mathf.Cos(EulerToRadian(xAngle));
        transform.position = Vector3.Lerp(transform.position, newPosition, scaleSensitivity * Time.deltaTime * 0.65f);

        transform.LookAt(lookAtPlayer);
        /*
        Vector3 lookAtPosition = (lookAtPlayer.position - transform.position).normalized;
        Quaternion lookAtRotation = Quaternion.LookRotation(lookAtPosition);
        transform.rotation =
            Quaternion.Slerp(transform.rotation, lookAtRotation, scaleSensitivity * Time.deltaTime);
        */
    }

    float EulerToRadian(float euler)
    {
        return euler * Mathf.PI / 180.0f;
    }
}
