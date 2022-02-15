using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeveledSkinController : MonoBehaviour, ISkinController
{
    public GameObject ActiveSkin
    {
        get => IsShowRoom ? showRoomSkin : skins[activeSkinIndex];
    }

    public bool IsShowRoom { get; set; } = false;
    [SerializeField] private List<GameObject> skins;
    [SerializeField] private GameObject showRoomSkin;
    
    private Dictionary<int, Rigidbody[]> rigidbodies = new Dictionary<int, Rigidbody[]>();
    private Dictionary<int, Animator> animators = new Dictionary<int, Animator>();

    private int activeSkinIndex = 0;

    public void SetSkin(ICharacterController characterController)
    {
        int level = 0;
        if(characterController is UpgradableCharacterController)
        {
            level = (characterController as UpgradableCharacterController).Level -1;
        }
        if (level >= skins.Count) level = skins.Count - 1;
        activeSkinIndex = level;
        for (int i = 0; i < skins.Count; i++)
        {
            skins[i].SetActive(i == level && !IsShowRoom);
            SetRigidbodies(i, true);
            SetAnimator(i, i == level && !IsShowRoom);
        }
        
        SetShowRoomSkinState();
    }

    private void SetShowRoomSkinState()
    {
        if(showRoomSkin == null || (!IsShowRoom && showRoomSkin == skins[skins.Count - 1])) return;
        showRoomSkin.SetActive(IsShowRoom);
        SetRigidbodies(-1, true, true);
        SetAnimator(-1, IsShowRoom, true);
    }

    private void SetRigidbodies(int index, bool value, bool isShowRoomSkin = false)
    {
        if (!rigidbodies.ContainsKey(index))
        {
            rigidbodies[index] = (isShowRoomSkin ? showRoomSkin : skins[index]).GetComponentsInChildren<Rigidbody>();
        }

        foreach (var item in rigidbodies[index])
        {
            item.isKinematic = value;
        }
    }

    private void SetAnimator(int index, bool value, bool isShowRoomSkin = false)
    {
        if (!animators.ContainsKey(index))
        {
            animators[index] = (isShowRoomSkin ? showRoomSkin : skins[index]).GetComponent<Animator>();
        }
        if(animators[index] != null)
            animators[index].enabled = value;
    }
}
