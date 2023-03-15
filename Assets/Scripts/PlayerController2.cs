using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController2 : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("移动速度")]
    public float MoveSpeed = 2.0f;
    [Tooltip("加速移动速度")]
    public float SprintSpeed = 5.335f;
    [Tooltip("旋转速度")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    [Tooltip("加速度")]
    public float SpeedChangeRate = 10.0f;

    [Space(10)]
    [Tooltip("跳跃高度")]
    public float JumpHeight = 1.2f;
    [Tooltip("重力，默认为 -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("跳跃间隔时间")]
    public float JumpTimeout = 0.50f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("当前是否在地面")]
    public bool Grounded = true;
    [Tooltip("粗糙地面偏移量")]
    public float GroundedOffset = -0.14f;
    [Tooltip("地面检测的半径，应该和CharacterController的半径匹配")]
    public float GroundedRadius = 0.28f;
    [Tooltip("地面有哪些层")]
    public LayerMask GroundLayers;

    [Header("CinemachineTarget")]
    [Tooltip("虚拟相机目标")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("最大仰角")]
    public float TopClamp = 80;
    [Tooltip("最小俯角")]
    public float BottomClamp = -80;
    [Tooltip("额外的角度来调整摄像机，用与当相机锁住的时候")]
    public float CameraAngleOverride = 0.0f;
    [Tooltip("相机灵敏度")]
    public float MouseSensitivity = 200;

    [Tooltip("虚拟相机")]
    public Cinemachine3rdPersonFollow CinemachineVirtualCamera;
    [Tooltip("相机缩放")]
    public float CameraDistance = 3;
    [Tooltip("相机缩放")]
    public float CameraDistanceRatio = 5;
    [Tooltip("相机缩放最小距离")]
    public float CameraDistanceMin = 2;
    [Tooltip("相机缩放最大距离")]
    public float CameraDistanceMax = 8;

    [Tooltip("相机锁")]
    public bool LockCameraPosition = false;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private float _cinemachineTargetDistance;

    // player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    // state
    private bool isRun = false;
    private bool isJump = false;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;

    private Animator _animator;
    private CharacterController _controller;
    private GameObject _mainCamera;

    private bool _hasAnimator;

    private void Awake()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start()
    {
        _hasAnimator = TryGetComponent(out _animator);
        _controller = GetComponent<CharacterController>();
        var cinemachine = FindObjectOfType<CinemachineVirtualCamera>();
        CinemachineVirtualCamera = cinemachine.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        AssignAnimationIDs();

        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRun = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LockCameraPosition = !LockCameraPosition;
        }

        _cinemachineTargetDistance -= Input.GetAxis("Mouse ScrollWheel") * CameraDistanceRatio;
        _cinemachineTargetDistance = Mathf.Clamp(_cinemachineTargetDistance, CameraDistanceMin, CameraDistanceMax);
    }

    private void FixedUpdate()
    {
        _hasAnimator = TryGetComponent(out _animator);

        JumpAndGravity();
        GroundedCheck();
        Move();
    }
    public float cameraDistanceLerpSpeed = 1;
    private void LateUpdate()
    {
        CameraPosition();
        CameraRotation();
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    /// <summary>
    /// 地面检测
    /// </summary>
    private void GroundedCheck()
    {
        // 得到检测点位置
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);

        // 检测结果
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        // 设置Animator
        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, Grounded);
        }
    }

    private void CameraPosition()
    {
        CameraDistance = Mathf.Lerp(CameraDistance, _cinemachineTargetDistance, Time.deltaTime * cameraDistanceLerpSpeed);
        CinemachineVirtualCamera.CameraDistance = CameraDistance;
    }

    private void CameraRotation()
    {
        var mouseX = Input.GetAxis("Mouse X") * MouseSensitivity;
        var mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity;

        _cinemachineTargetYaw += mouseX * Time.deltaTime;
        _cinemachineTargetPitch -= mouseY * Time.deltaTime;

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
        var targetRot = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0);
        CinemachineCameraTarget.transform.rotation = targetRot;
    }

    /// <summary>
    /// 角色移动
    /// </summary>
    private void Move()
    {
        // 获取到当前的输入向量
        Vector3 curInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // 根据是否按下加速键，获得最终速度值
        float targetSpeed = isRun ? SprintSpeed : MoveSpeed;

        // 如果输入的数值太小则不计算
        if (curInput == Vector3.zero) targetSpeed = 0.0f;

        // 获取玩家当前在水平面上的单位速度
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        //float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

        // 模拟加速过程
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // 模拟一个非线性的加速过程
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * curInput.magnitude, Time.fixedDeltaTime * SpeedChangeRate);

            // 精确到小数点后3位
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        // 设置动画的速度
        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.fixedDeltaTime * SpeedChangeRate);

        // 获取玩家的输入单位水平向量
        Vector3 inputDirection = new Vector3(curInput.x, 0.0f, curInput.z).normalized;

        // Vector2's != 更节省性能
        if (curInput != Vector3.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            // 旋转到相对于摄像机的方向
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        // 获取移动的目标方向
        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // 移动玩家，水平面的移动 + 垂直方向的移动
        _controller.Move(targetDirection.normalized * (_speed * Time.fixedDeltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        // 设置Animator
        if (_hasAnimator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, curInput.magnitude);
        }
    }

    /// <summary>
    /// 角色跳跃
    /// </summary>
    private void JumpAndGravity()
    {
        // 在地面上
        if (Grounded)
        {
            // 重置下落时间
            _fallTimeoutDelta = FallTimeout;

            // 设置Animator
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump, false);
                _animator.SetBool(_animIDFreeFall, false);
            }

            // 快速下落
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // 如果还在跳跃
            if (isJump && _jumpTimeoutDelta <= 0.0f)
            {
                // 求出垂直速度
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                // 设置Animator
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, true);
                }
            }

            // 跟新跳跃的状态数值
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.fixedDeltaTime;
            }
        }
        //在半空中
        else
        {
            // 重置跳跃时间
            _jumpTimeoutDelta = JumpTimeout;

            // 如果还在下落
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.fixedDeltaTime;
            }
            else
            {
                // 设置Animator
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDFreeFall, true);
                }
            }

            // 在半空中不能跳跃
            isJump = false;
        }

        // 跟新速度为 Vt = V0 + a * t
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.fixedDeltaTime;
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
    }

}
