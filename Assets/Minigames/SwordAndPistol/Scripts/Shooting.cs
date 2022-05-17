using UnityEngine;

namespace Minigames.SwordAndPistol.Scripts
{
    public class Shooting : MonoBehaviour
    {
        [Header("Shooting Button")]
        [SerializeField] private OVRInput.Button shootingButton;
        // [Tooltip("Specifies which hand the object is held")][SerializeField] private OVRInput.Controller handController;
        
        [Space]
        [SerializeField] private PoolType bulletType;
        [SerializeField] private Transform nozzleTransform;
        [SerializeField] private Animator gunAnimator;
        [SerializeField] private float fireRate = 0.1f;
        
        private static readonly int Fire = Animator.StringToHash("Fire");
        private float elapsedTime;

        private Collider SlicerCollider => slicerCollider ??= FindObjectOfType<Slicer>().GetComponent<Collider>();
        private Collider slicerCollider;

        // Update is called once per frame
        private void Update()
        {
            //elapsed time
            elapsedTime += Time.deltaTime;

            if (/*Input.GetMouseButtonDown(0) ||*/ OVRInput.GetDown(shootingButton))
            {
                if (elapsedTime > fireRate)
                {
                    Shoot();
                    elapsedTime = 0;
                }
            }
        }

        
        private void Shoot()
        {
            //Play sound
            AudioManager.instance.gunSound.gameObject.transform.position = nozzleTransform.position;
            AudioManager.instance.gunSound.Play();

            //Play animation
            gunAnimator.SetTrigger(Fire);

            //Create the bullet
            var bullet = PoolingSystem.Instance.Create<Bullet>(bulletType);
            bullet.transform.position = nozzleTransform.position;
            bullet.transform.rotation = Quaternion.Euler(0, 0, 0);
            bullet.StartMovement(nozzleTransform.forward);
            
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), SlicerCollider);
        }
    }
}
