using Game.Frame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraMovementMono : MonoBehaviour
{
    private static Quaternion lastRotation; 

    [Header("旋转的目标")]
    public Transform Target;

    [Header("X 轴旋转速度")]
    public float SpeedX = 200;

    [Header("Y 轴旋转速度")]
    public float SpeedY = 200;

    [Header("最大速度")]
    public float MaxSpeed = 10;

    [Header("钳制角度")]
    public Vector2 LimitY = new Vector2(20, 50);

    [Header("当前距离")]
    public float Distance = 2;

    [Header("钳制距离")]
    public Vector2 DistanceRange = new Vector2(2, 8);

    [Header("需要阻尼")]
    public bool needDamping = true;

    [Header("阻尼")]
    private float damping = 5.0f;

    private float x = 0.0f;
    private float y = 0.0f;

    public Camera UICamera;
    private Camera mainCamera;

    public bool IsRunning = true;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        foreach (var cam in Camera.allCameras)
        {
            if (cam.name.IndexOf("UICamera") >= 0)
            {
                UICamera = cam;
            }
        }

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    private void OnEnable()
    {
        transform.rotation = lastRotation;

        /// 初始化相机
        AppFacade.Timer.Wait(0, () =>
        {
            UICamera.GetComponent<UniversalAdditionalCameraData>().renderType = CameraRenderType.Overlay;
            var stack = mainCamera.GetComponent<UniversalAdditionalCameraData>().cameraStack;
            stack.Remove(UICamera);
            stack.Add(UICamera);
        });
    }

    private void OnDisable()
    {
        /// 禁用相机
        var stack = mainCamera.GetComponent<UniversalAdditionalCameraData>().cameraStack;
        stack.Remove(UICamera);
        UICamera.GetComponent<UniversalAdditionalCameraData>().renderType = CameraRenderType.Base;

        lastRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        if (Target)
        {
            if (Input.GetMouseButton(1) && IsRunning)
            {
                x += Input.GetAxis("Mouse X") * SpeedX * 0.02f;
                y -= Input.GetAxis("Mouse Y") * SpeedY * 0.02f;

                y = ClampAngle(y, LimitY.x, LimitY.y);
            }
            Distance -= Input.GetAxis("Mouse ScrollWheel") * MaxSpeed;
            Distance = Mathf.Clamp(Distance, DistanceRange.x, DistanceRange.y);

            Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
            Vector3 disVector = new Vector3(0.0f, 0.0f, -Distance);
            Vector3 position = rotation * disVector + Target.position;

            if (needDamping)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
                transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * damping);
            }
            else
            {
                transform.rotation = rotation;
                transform.position = position;
            }
        }
    }

    public void Run()
    {
        IsRunning = true;
    }

    public void Stop()
    {
        IsRunning = false;
    }

    /// <summary>
    /// 牵制角度
    /// </summary>
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
