using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPoint : MonoBehaviour
{
    public void DestroyCharacter(GameObject character)
    {
        StartCoroutine(DestroyWithDelay(character));
    }

    private IEnumerator DestroyWithDelay(GameObject character)
    {
        yield return new WaitForSeconds(0.001f);
        // EventService.Instance.OnCustomerCountChanged.Invoke(false);
        ICharacterController characterController = character.GetComponent<ICharacterController>();
        PoolingSystem.Instance.Destroy(characterController.GetCharacterValue<PoolType>(CharacterDataType.PoolType), character);
    }
}
