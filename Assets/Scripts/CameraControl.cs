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
    public Transform rebornPosition;
    public GameObject MainCharacter;
    public GameObject Alice;
    
    private FloatingJoystick _rotateJoystick;
    private FloatingJoystick _moveJoystick;
    private GameObject _joystickObjectRotate;
    private GameObject _joystickObjectMove;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("CharacterRecord"))
        {
            if (PlayerPrefs.GetInt("CharacterRecord") == 1)
                Instantiate(MainCharacter, rebornPosition.position, Quaternion.identity);
            else
                Instantiate(Alice, rebornPosition.position, Quaternion.identity);
        }
        else
        {
            Instantiate(MainCharacter, rebornPosition.position, Quaternion.identity);
        }
        if (lookAtPlayer != null)
            ControlViewport();
        if (_rotateJoystick == null)
            FindJoystick();
        SetJoystick(Application.platform == RuntimePlatform.Android);
    }

    // Update is called once per frame
    void Update()
    {
        if (_rotateJoystick == null)
            FindJoystick();
        if (lookAtPlayer != null)
            ControlViewport();
    }

    void ControlViewport()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor)
        {
            // windows control
            // 滾輪放大縮小遊戲畫面
            yDistance -= Input.GetAxis("Mouse ScrollWheel") * scaleSensitivity;
            // 按住右鍵時
            if (Input.GetMouseButton(1))
            {
                // 隱藏游標
                Cursor.visible = false;
                // 拖曳滑鼠調整視角
                xAngle -= Input.GetAxis("Mouse X") * xAngleSensitivity;
                yAngle -= Input.GetAxis("Mouse Y") * yAngleSensitivity;
            }
            else
            {
                // 顯示游標
                Cursor.visible = true;
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            // Android Control
            if (_rotateJoystick != null)
            {
                // rotate viewport
                if (_rotateJoystick.Horizontal >= 0.5f || _rotateJoystick.Horizontal <= -0.5f)
                    xAngle -= (_rotateJoystick.Horizontal * xAngleSensitivity / 2);
                if (_rotateJoystick.Vertical >= 0.5f || _rotateJoystick.Vertical <= -0.5f)
                    yAngle -= _rotateJoystick.Vertical;
                // scale viewport
                if (Input.touchCount == 2 &&
                    !_joystickObjectRotate.activeSelf &&
                    !_joystickObjectMove.activeSelf)
                {
                    // 紀錄兩指的位置與短時間內的移動變量
                    Vector2 finger1 = Input.touches[0].position;
                    Vector2 moveFinger1 = finger1 + Input.touches[0].deltaPosition;
                    Vector2 finger2 = Input.touches[1].position;
                    Vector2 moveFinger2 = finger2 + Input.touches[1].deltaPosition;
                    // [紀錄位置+移動變量].length - [紀錄位置].length
                    float fingerDistance = (finger1 - finger2).magnitude;
                    float moveFingerDistance = (moveFinger1 - moveFinger2).magnitude;
                    yDistance -= ((moveFingerDistance - fingerDistance) / (scaleSensitivity * 2));

                }
            }
        }
        // [xAngle] 輪迴一圈、將數值限定在[0, 360]
        xAngle = xAngle > 360 ? 0 : xAngle;
        xAngle = xAngle < 0 ? 360 : xAngle;
        yAngle = Mathf.Clamp(yAngle, minYAngle, maxYAngle);
        yDistance = Mathf.Clamp(yDistance, minDistance, maxDistance);
        float xDistance = yDistance * Mathf.Cos(EulerToRadian(yAngle));
        // 計算相機新位置
        Vector3 newPosition = lookAtPlayer.position +
                              Vector3.forward * xDistance * Mathf.Sin(EulerToRadian(xAngle)) +
                              Vector3.up * yDistance * Mathf.Sin(EulerToRadian(yAngle)) +
                              Vector3.right * xDistance * Mathf.Cos(EulerToRadian(xAngle));
        // 線性移動至新位置
        transform.position = Vector3.Lerp(transform.position, newPosition, scaleSensitivity * Time.deltaTime * 0.65f);

        transform.LookAt(lookAtPlayer);

        /*
        // look at by slerp
        Vector3 lookAtPosition = (lookAtPlayer.position - transform.position).normalized;
        Quaternion lookAtRotation = Quaternion.LookRotation(lookAtPosition);
        transform.rotation =
            Quaternion.Slerp(transform.rotation, lookAtRotation, scaleSensitivity * Time.deltaTime);
        */
    }

    private float EulerToRadian(float euler)
    {
        return euler * Mathf.PI / 180.0f;
    }

    private void FindJoystick()
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

    private void SetJoystick(bool onOff)
    {
        if (_moveJoystick != null && _rotateJoystick != null)
        {
            _moveJoystick.gameObject.SetActive(onOff);
            _rotateJoystick.gameObject.SetActive(onOff);
        }
    }
}
