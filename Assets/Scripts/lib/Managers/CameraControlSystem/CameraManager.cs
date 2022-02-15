using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraManager : Singleton<CameraManager>
{
    private Camera idleScreenCam; 
    private Camera inGameCam;
    private CameraType activeCameraType = CameraType.IdleScreenCamera;
    private Dictionary<CameraType, Camera> cameras = new Dictionary<CameraType, Camera>();
    // private FollowedObjectByCamera MainCharacterFollowedObjectByCamera => mainCharacterFollowedObjectByCamera ??= InteractionManager.Instance.GetMainInteractor().CarryController.GetComponent<FollowedObjectByCamera>();
    // private FollowedObjectByCamera mainCharacterFollowedObjectByCamera;
    // TODO Emiran : Level gecislerinde camerayı kontrol et
    
    public void ChangeActiveCamera(CameraType cameraType)
    {
        foreach (var item in cameras)
        {
            if(item.Value != null)
                item.Value.enabled = cameraType == item.Key;
        }
        activeCameraType = cameraType;
    }

    // public void OnEnable()
    // {
    //     LevelManager.Instance.BeforeLevelLoad.AddListener (() => { mainCharacterFollowedObjectByCamera = null; });
    // }
    //
    // public void OnDisable()
    // {
    //     if(LevelManager.IsAvailable())
    //         LevelManager.Instance.BeforeLevelLoad.RemoveListener(() => { mainCharacterFollowedObjectByCamera = null; });
    // }

    public void SetCamera(CameraType cameraType, Camera camera)
    {
        cameras[cameraType] = camera;
    }    
    
    public Camera GetCamera(CameraType cameraType)
    {
        if (cameras.ContainsKey(cameraType))
            return cameras[cameraType];

        return null;
    }

    public Camera GetCurrentCamera()
    {
        return GetCamera(activeCameraType);
    }
    
    public void ShowWithCam(GameObject go, float waitForCamDelay = 2)
    {
        // MainCharacterFollowedObjectByCamera.enabled = false;
        var fo = GetComponent<FollowedObjectByCamera>();
        if (fo == null) fo = go.AddComponent<FollowedObjectByCamera>();
        fo.enabled = true;
        fo.FollowThisObject();
        StartCoroutine(WaitForCam(fo, waitForCamDelay));
    }

    private IEnumerator WaitForCam(FollowedObjectByCamera fo, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        fo.enabled = false;
        // MainCharacterFollowedObjectByCamera.enabled = true;
        // MainCharacterFollowedObjectByCamera.FollowThisObject();
    }
}

public enum CameraType
{
    IdleScreenCamera = 0,
    InGameCamera = 1,
    InGameCameraTP = 2,
    InGameFront=3,
    InGameCross=4,
    InGamePlayable=5,
}