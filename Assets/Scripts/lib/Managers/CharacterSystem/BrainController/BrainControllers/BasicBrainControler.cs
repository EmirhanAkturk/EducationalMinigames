using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBrainControler : MonoBehaviour, IBrainController
{
    [SerializeField] private float movementRange;

    protected ICharacterController characterController;
    protected Vector3 destinationPos;
    protected bool isWalking;
    
    public virtual void Initialize(ICharacterController newCharacterController)
    {
        characterController = newCharacterController;
        characterController.SetSpeed(characterController.GetCharacterValue<float>(CharacterDataType.MoveSpeed));
    }

    public virtual void Do()
    {
        MoveRandomPos();
    }

    public void MoveRandomPos()
    {
        if (!isWalking)
        {
            destinationPos = RandomPosGenerate(new Vector3(-movementRange, 0f, -movementRange),
                new Vector3(movementRange, 0f, movementRange));
            characterController.Move(destinationPos);
            isWalking = true;
        }
        else
        {
            if (Vector3.Distance(transform.position, destinationPos) < 0.1f)
            {
                isWalking = false;
            }
        }
    }

    public Vector3 RandomPosGenerate(Vector3 min, Vector3 max)
    {
        Vector3 newPos;
        newPos = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));

        return newPos;
    }

}
