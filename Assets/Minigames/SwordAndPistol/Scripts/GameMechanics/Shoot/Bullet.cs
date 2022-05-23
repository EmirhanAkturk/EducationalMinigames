using System;
using UnityEngine;

namespace Minigames.SwordAndPistol.Scripts
{
    public class Bullet : MonoBehaviour
    {
        // Start is called before the first frame update

        //bullet speed
        [SerializeField] private float speed = 10.5f;
        //rigidbody of the bullet

        private Rigidbody rb;
        private TrailRenderer trailRenderer;
        private DestroyAfterSeconds destroyAfterSeconds;

        public void StartMovement(Vector3 direction)
        {
            transform.forward = direction;
            rb.velocity = direction * speed;
        }
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            trailRenderer = GetComponent<TrailRenderer>();
            destroyAfterSeconds = GetComponent<DestroyAfterSeconds>();
        }

        private void OnEnable()
        {
            rb.isKinematic = false;
        }

        private void OnDisable()
        {
            ResetBulletMovement();
        }

        private void ResetBulletMovement()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;

            trailRenderer.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            destroyAfterSeconds.DestroyObject();
        }
    }
}
