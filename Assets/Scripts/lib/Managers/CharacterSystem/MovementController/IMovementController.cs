using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementController
{
    void Move(Vector3 destination, ICharacterController characterController, Action callback = null);
    void MoveWithDirection(Vector2 direction, ICharacterController characterController);
    void Stop();
    void SetSpeed(float newSpeed);
    void SetMovementSetting(float stoppingDistance);
    float GetSpeed();
    bool IsCharacterWalking();
}
