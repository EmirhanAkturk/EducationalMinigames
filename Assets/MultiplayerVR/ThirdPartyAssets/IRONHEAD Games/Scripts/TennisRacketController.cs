using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minigames.SwordAndPistol.Scripts;

public class TennisRacketController : MonoBehaviour
{
    private Rigidbody Rb => rb ??= GetComponent<Rigidbody>();
    private Rigidbody rb;

    [SerializeField] private float multiplier = 1.25f;


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collide");
        if (collision.gameObject.CompareTag("TennisBall"))
        {
            AudioManager.Instance.PlaySound(Minigames.SwordAndPistol.Scripts.AudioType.PinponSound, collision.transform.position);

            Debug.Log("tennisball collide");
            var tennisBallRb = collision.gameObject.GetComponent<Rigidbody>();

            //tennisBallRb.angularVelocity = Vector3.zero;
            tennisBallRb.angularVelocity = -tennisBallRb.angularVelocity;
            tennisBallRb.velocity = -tennisBallRb.velocity * multiplier;
        }
    }
}
