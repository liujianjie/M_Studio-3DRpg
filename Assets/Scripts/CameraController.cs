using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 m_Camera;
    public Transform target;
    public float targetHeight = 1.8f;
    public float targetSide = 0.1f;
    public float distance = 4;
    public float maxDistance = 8;
    public float minDistance = 2.2f;
    public float xSpeed = 250;
    public float ySpeed = 125;
    public float yMinLimit = -10;
    public float yMaxLimit = 72;
    public float zoomRate = 80;
    private float x = 20;
    private float y = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }
        m_Camera.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse ScrollWheel"));

        x += m_Camera.x * xSpeed * Time.deltaTime;
        y -= m_Camera.y * ySpeed * Time.deltaTime;
        y = clampAngle(y, yMinLimit, yMaxLimit);
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        transform.rotation = rotation;

        distance -= (m_Camera.z * Time.deltaTime) * zoomRate * Mathf.Abs(distance);
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        transform.position = target.position + new Vector3(0, targetHeight, 0) + rotation * (new Vector3(targetSide, 0, -1) * distance);
    }

    float clampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }
}

