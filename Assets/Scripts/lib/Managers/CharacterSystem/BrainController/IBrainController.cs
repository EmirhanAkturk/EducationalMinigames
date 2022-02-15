using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBrainController
{
    void Initialize(ICharacterController characterController);
    void Do();
}


