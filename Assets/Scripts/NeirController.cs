using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NeirController : MonoBehaviour
{

    [Header("ÐéÄâÏà»ú")]
    [Tooltip("")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("")]
    public float TopClamp = 70.0f;
    [Tooltip("")]
    public float BottomClamp = -30.0f;
    [Tooltip("")]
    public float CameraAngleOverrride = 0.0f;
    [Tooltip("")]
    public bool LockCAmeraPosition = false;

    //cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private NeirInputs _input;
    private GameObject _mainCamera;

    private const float _threshold = 0.01f;

    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start()
    {
        _input = GetComponent<NeirInputs>();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        if (_input.look.sqrMagnitude >= _threshold && !LockCAmeraPosition)
        {
            _cinemachineTargetYaw += _input.look.x * Time.deltaTime;
            _cinemachineTargetPitch += _input.look.y * Time.deltaTime;
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverrride, _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}

