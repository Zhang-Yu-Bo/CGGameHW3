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

    // Update is called once per frame
    void Update()
    {
        if (lookAtPlayer != null)
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
                Cursor.visible = false;
                xAngle -= Input.GetAxis("Mouse X") * xAngleSensitivity;
                yAngle -= Input.GetAxis("Mouse Y") * yAngleSensitivity;
            }
            else
            {
                Cursor.visible = true;
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
