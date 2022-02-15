using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkinController : MonoBehaviour, ISkinController
{
    private string modelFolderPath = "CharacterModels/";
    private string modelPath;

    public GameObject ActiveSkin { get; private set; }

    public void SetSkin(ICharacterController characterController)
    {
        ActiveSkin = GetComponentInChildren<BasicAnimationController>().gameObject;
        /*string characterModel = characterController.GetCharacterValue<string>(CharacterDataType.ModelName);
        BasicAnimationController[] basicAnimationControllers = transform.GetComponentsInChildren<BasicAnimationController>();
        
        if (basicAnimationControllers.Length > 1)
        {
            foreach (var animationController in basicAnimationControllers)
            {
                Destroy(animationController.gameObject);
            }
        }

        var tmpBody = transform.GetComponentInChildren<BasicAnimationController>();

        Transform oldCharacterModelTr = null;
        if(tmpBody != null) oldCharacterModelTr = tmpBody.transform;
        
        if (oldCharacterModelTr != null && oldCharacterModelTr.gameObject.name.Equals(characterModel))
        {
            foreach (var item in oldCharacterModelTr.GetComponentsInChildren<Rigidbody>())
            {
                item.isKinematic = true;
            }
            return; //Character Model same, do nothing
        }
            
        modelPath = modelFolderPath + characterController.GetCharacterValue<string>(CharacterDataType.ModelPath) + "/"+ characterModel;
        GameObject modelPrefab =  Resources.Load<GameObject>(modelPath);
        try
        {
            GameObject newCharacterModel = Instantiate(modelPrefab, transform.position, Quaternion.identity);
            newCharacterModel.name = characterModel;
            newCharacterModel.transform.parent = transform;
            ActiveSkin = newCharacterModel;

            if (oldCharacterModelTr != null) Destroy(oldCharacterModelTr.gameObject);

            foreach (var item in newCharacterModel.GetComponentsInChildren<Rigidbody>())
            {
                item.isKinematic = true;
            }
        }
        catch(Exception e)
        {
            Debug.Log("Missing Prefab: " + characterModel);
        }*/
        
    }
}
