using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraSkinController : MonoBehaviour, ISkinController
{
    public GameObject ActiveSkin => skins[activeSkinIndex];

    [SerializeField] private List<GameObject> skins;

    private readonly Dictionary<int, Rigidbody[]> rigidbodies = new Dictionary<int, Rigidbody[]>();
    private readonly Dictionary<int, Animator> animators = new Dictionary<int, Animator>();

    public List<GameObject> Skins => skins;
    
    private int activeSkinIndex = 0;

    public void SetSkin(ICharacterController characterController)
    {
        int modelIndex = 0; //StaffService.Instance.IsSuperStaff(characterController) ? 1 : 0;
        activeSkinIndex = modelIndex;
        for (int i = 0; i < skins.Count; i++)
        {
            bool isActiveModel = (i == modelIndex);
            skins[i].SetActive(isActiveModel);
            SetRigidbodies(i, isActiveModel);
            SetAnimator(i, isActiveModel);
        }
    }

    private void SetRigidbodies(int idx, bool value)
    {
        if (!rigidbodies.ContainsKey(idx))
            rigidbodies[idx] = skins[idx].GetComponentsInChildren<Rigidbody>();

        foreach (var rb in rigidbodies[idx])
            rb.isKinematic = value;
    }

    private void SetAnimator(int idx, bool value)
    {
        if (!animators.ContainsKey(idx))
            animators[idx] = skins[idx].GetComponent<Animator>();

        if (animators[idx] != null)
            animators[idx].enabled = value;
    }
}
