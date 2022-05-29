using Minigames.SwordAndPistol.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NerfGunController : MonoBehaviour
{

    [Header("Shooting Button")]
    [SerializeField] private InputActionReference gunShooterLeftAction;
    [SerializeField] private InputActionReference gunShooterRightAction;
    // [Tooltip("Specifies which hand the object is held")][SerializeField] private OVRInput.Controller handController;

    [Space]
    [SerializeField] private PoolType bulletType;
    [SerializeField] private Transform nozzleTransform;
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private float fireRate = 0.1f;


    private static readonly int Fire = Animator.StringToHash("Fire");
    private float elapsedTime;

    //private Collider SlicerCollider => slicerCollider ??= FindObjectOfType<Slicer>().GetComponent<Collider>();
    //private Collider slicerCollider;

    private int? layerMask;

    private void OnEnable()
    {
        gunShooterLeftAction.action.performed += ButtonListener;
        gunShooterRightAction.action.performed += ButtonListener;
    }
    private void OnDisable()
    {
        gunShooterLeftAction.action.performed -= ButtonListener;
        gunShooterRightAction.action.performed -= ButtonListener;
    }

    // Update is called once per frame
    private void Update()
    {
        //elapsed time
        elapsedTime += Time.deltaTime;

        //layerMask ??= LayerMask.NameToLayer("InHand");
        //Debug.Log("gameObject.layer.Equals(layerMask) : " + gameObject.layer.Equals(layerMask));
        //Debug.Log("(OVRInput.GetDown(shootingButton1) || OVRInput.GetDown(shootingButton2) " + ((OVRInput.GetDown(shootingButton1) || OVRInput.GetDown(shootingButton2))));
        //if (gameObject.layer.Equals(layerMask) && (OVRInput.GetDown(shootingButton1) || OVRInput.GetDown(shootingButton2)))
        //{
        //    if (elapsedTime > fireRate )
        //    {
        //        Shoot();
        //        elapsedTime = 0;
        //    }
        //}
    }


    private void Shoot()
    {
        //Play sound
        AudioManager.Instance.PlaySound(Minigames.SwordAndPistol.Scripts.AudioType.GunSound, nozzleTransform.position);

        //Play animation
        if(gunAnimator != null) gunAnimator.SetTrigger(Fire);

        //Create the bullet
        var bullet = PoolingSystem.Instance.Create<Bullet>(bulletType);
        bullet.transform.position = nozzleTransform.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, 0);
        bullet.StartMovement(nozzleTransform.forward);

        //Physics.IgnoreCollision(bullet.GetComponent<Collider>(), SlicerCollider);
    }

    private void ButtonListener(InputAction.CallbackContext obj)
    {
        layerMask ??= LayerMask.NameToLayer("InHand");
        if (gameObject.layer.Equals(layerMask))
        {
            if (elapsedTime > fireRate)
            {
                Shoot();
                elapsedTime = 0;
            }
        }
    }
}
