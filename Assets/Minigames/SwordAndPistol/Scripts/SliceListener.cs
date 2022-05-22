using System.Collections;
using System.Collections.Generic;
using Minigames.SwordAndPistol.Scripts;
using UnityEngine;

public class SliceListener : MonoBehaviour
{
    public Slicer slicer;
    private void OnTriggerEnter(Collider other)
    {
        slicer.IsTouched = true;
    }
}
