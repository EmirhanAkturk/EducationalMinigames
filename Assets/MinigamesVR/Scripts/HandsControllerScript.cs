using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MinigamesVR.Scripts
{
    public class HandsControllerScript : MonoBehaviour
    {
        [SerializeField] private InputActionReference gripInputAction;
        [SerializeField] private InputActionReference triggerInputAction;

        private Animator handAnimator;
        private static readonly int Trigger = Animator.StringToHash("Trigger");
        private static readonly int Grip = Animator.StringToHash("Grip");

        private void Awake()
        {
            handAnimator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            gripInputAction.action.performed += GripPressed;
            triggerInputAction.action.performed += TriggerPressed;
        }

        private void OnDisable()
        {
            gripInputAction.action.performed -= GripPressed;
            triggerInputAction.action.performed -= TriggerPressed;
        }
        
        private void GripPressed(InputAction.CallbackContext obj)
        {
            handAnimator.SetFloat(Grip, obj.ReadValue<float>());
        }

        private void TriggerPressed(InputAction.CallbackContext obj)
        {
            handAnimator.SetFloat(Trigger, obj.ReadValue<float>());
        }

    }
}
