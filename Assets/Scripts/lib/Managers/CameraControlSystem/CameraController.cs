using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    [SerializeField] private float offsetZ;
    [SerializeField] private CameraType cameraType;

    private float initialX;
    private float initialY;
    private float initialZ;

    public Transform TargetObject
    {
        get => targetObject;
        set
        {
            targetObject = value;
            if (value != null)
            {
                var position = value.position;
               /* initialX = position.x;
                initialY = position.y;
                initialZ = position.z;*/
            }
        }
    }

    private Camera ThisCamera => thisCamera ??= GetComponent<Camera>();
    private Transform targetObject;
    private Camera thisCamera;
    
    private void OnEnable()
    {
        if (CameraManager.Instance.GetCamera(cameraType) != ThisCamera)
            CameraManager.Instance.SetCamera(cameraType, ThisCamera);
        if (CameraManager.Instance.GetCurrentCamera() != ThisCamera)
            ThisCamera.enabled = false;
        if(cameraType == ConfigurationService.Configurations.IngameCameraType)
            CameraManager.Instance.ChangeActiveCamera(cameraType);
    }

    private float lerpConst = 0.1f;
    private void Update()
    {
        if (ThisCamera.enabled && TargetObject != null)
        {
            float newX = offsetX + initialX + TargetObject.transform.position.x;
            float newY = offsetY + initialY;
            float newZ = offsetZ + initialZ + TargetObject.transform.position.z;

            var targetPos = new Vector3(newX, newY, newZ);
            if(Vector3.Distance(targetPos, transform.position) > 1f)
            {
                lerpConst = 0.95f;
            }
            else
            {
                if (lerpConst > 0.1f) lerpConst -= 0.01f;
            }
            transform.position = Vector3.Lerp(targetPos, transform.position, lerpConst);
        }
    }
}
