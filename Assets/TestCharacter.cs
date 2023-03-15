using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    float zRotation = 0f;
    private float moveSpeed = 5.0f; // 移动速度
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseX;
        zRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        zRotation = Mathf.Clamp(zRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, zRotation);
        //transform.Rotate(Vector3.up * mouseX);

        // 主角移动逻辑
        float input_h = Input.GetAxis("Horizontal");
        float input_v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * input_h + transform.forward * input_v;
        Vector3 moveDir = new Vector3(input_h, 0.0f, input_v);
        if (moveDir != Vector3.zero)
        {
            transform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.World);
            //if (rigidbody.velocity.magnitude < moveSpeed)
            //{
            //    rigidbody.AddForce(moveDir * 100f);// 解决抖动问题
            //}
        }
    }
}
