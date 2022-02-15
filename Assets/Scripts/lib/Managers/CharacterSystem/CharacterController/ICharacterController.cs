using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface ICharacterController
{
    T GetCharacterValue<T>(CharacterDataType characterDataType);
    Transform CharacterModelTr { get; }

    void Move(Vector3 destination, Action callback = null);
    void MoveWithDirection(Vector2 direction);
    void LookTargetObject(Vector3 targetPos);
    void Stop();
    void SetSpeed(float newSpeed);
    void SetSkin();
	float GetSpeed();
    bool IsCharacterWalking();
}
