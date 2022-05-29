using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minigames.SwordAndPistol.Scripts;
public class BasketballController : MonoBehaviour
{
    private float lastPlayTime;
    private float soundPlayPeriod = 0.2f;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Floor")) return;
        if (Time.time < lastPlayTime + soundPlayPeriod) return;
        lastPlayTime = Time.time;
        AudioManager.Instance.PlaySound(Minigames.SwordAndPistol.Scripts.AudioType.BasketballSound, collision.transform.position);
    }
}
