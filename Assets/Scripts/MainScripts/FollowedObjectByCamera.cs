using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowedObjectByCamera : MonoBehaviour
{
    private Camera _camera;

    public void FollowThisObject()
    {
        _camera = CameraManager.Instance.GetCurrentCamera();
        if (_camera == null)
        {
            StartCoroutine(wait(AssignCamera));
        }
        else
        {
            AssignCamera();
        }
    }

    private void OnDisable()
    {
        if(_camera != null)
        {
            CameraController cameraController = _camera.GetComponent<CameraController>();
            if (cameraController != null)
                cameraController.TargetObject = null;

            _camera = null;
        }
    }

    private IEnumerator wait(Action callback)
    {
        while (_camera == null)
        {
            yield return new WaitForEndOfFrame();
            _camera = CameraManager.Instance.GetCurrentCamera();
        }
        callback();
    }

    private void AssignCamera()
    {
        CameraController cameraController = _camera.GetComponent<CameraController>();

        if (cameraController != null)
            cameraController.TargetObject = gameObject.transform;
    }
}
