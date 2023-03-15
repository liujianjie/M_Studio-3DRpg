using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTest : MonoBehaviour
{
    private Transform cam;
    public float speed = 4f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float horizontal;
    float vertical;

    Animator anim;
    private Rigidbody rb;
    void Start()
    {
        cam = Camera.main.transform;
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
        if ((dir.magnitude >= 0.1f))
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            this.rb.velocity = this.rb.velocity.y * Vector3.up + moveDir * speed;
        }
        else
        {
            this.rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        playAni(dir);
    }

    private void LateUpdate()
    {
        //this.transform.position = this.rb.transform.position;
    }

    void playAni(Vector3 vec)
    {
        //anim.SetFloat("horizontal", Mathf.Abs(horizontal));
        //anim.SetFloat("vertical", Mathf.Abs(vertical));
        anim.SetFloat("Speed", vec.magnitude);// 根据速度，判定是走还是跑
    }
}

