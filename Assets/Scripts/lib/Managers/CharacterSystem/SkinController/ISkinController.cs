using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISkinController
{
    GameObject ActiveSkin { get; }
    void SetSkin(ICharacterController characterController);
}
