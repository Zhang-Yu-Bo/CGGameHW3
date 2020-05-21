using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform lookAtPlayer;
    public float yDistance;
    public float maxDistance;
    public float minDistance;
    public float xAngle;
    public float yAngle;
    public float maxYAngle;
    public float minYAngle;
    public float xAngleSensitivity;
    public float yAngleSensitivity;
    public float scaleSensitivity;

    // Start is called before the first frame update
    void Start()
    {
        // 移動相機到指定物件的後面，並且朝向指定物件
        transform.position = lookAtPlayer.position + 
                             -lookAtPlayer.forward * yDistance * Mathf.Cos(EulerToRadian(yAngle)) +
                             lookAtPlayer.up * yDistance * Mathf.Sin(EulerToRadian(yAngle));
        transform.LookAt(lookAtPlayer);
        
    }

    // Update is called once per frame
    void Update()
    {
        ControlViewport();
    }

    void ControlViewport()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor)
        {
            yDistance -= Input.GetAxis("Mouse ScrollWheel") * scaleSensitivity;
            if (Input.GetMouseButton(1))
            {
                xAngle -= Input.GetAxis("Mouse X") * xAngleSensitivity;
                yAngle -= Input.GetAxis("Mouse Y") * yAngleSensitivity;
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

        transform.position = lookAtPlayer.position +
                             lookAtPlayer.forward * xDistance * Mathf.Sin(EulerToRadian(xAngle)) +
                             lookAtPlayer.up * yDistance * Mathf.Sin(EulerToRadian(yAngle)) +
                             lookAtPlayer.right * xDistance * Mathf.Cos(EulerToRadian(xAngle));

        transform.LookAt(lookAtPlayer);
    }

    float EulerToRadian(float euler)
    {
        return euler * Mathf.PI / 180.0f;
    }
}
