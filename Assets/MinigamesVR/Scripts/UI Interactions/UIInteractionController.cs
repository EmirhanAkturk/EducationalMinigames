using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace MiniGamesVR.Scripts.UI_Interactions
{
    public class UIInteractionController : MonoBehaviour
    {
        [SerializeField] private GameObject uiController;
        [SerializeField] private GameObject baseController;
        [SerializeField] private InputActionReference inputActionReferenceUISwitcher;
        [SerializeField] private GameObject uiCanvasObject;
        [SerializeField] private Vector3 positionOffsetForUICanvasObject;

        private bool isUICanvasActive = false;
        
        private void OnEnable()
        {
            inputActionReferenceUISwitcher.action.performed += ActivateUIMode;
        }
        private void OnDisable()
        {
            inputActionReferenceUISwitcher.action.performed -= ActivateUIMode;
        }

        private void Start()
        {
            //Deactivating UI Canvas Gameobject by default
            uiCanvasObject.SetActive(false);

            //Deactivating UI Controller by default
            uiController.GetComponent<XRRayInteractor>().enabled = false;
            uiController.GetComponent<XRInteractorLineVisual>().enabled = false;
        }

        /// <summary>
        /// This method is called when the player presses UI Switcher Button which is the input action defined in Default Input Actions.
        /// When it is called, UI interaction mode is switched on and off according to the previous state of the UI Canvas.
        /// </summary>
        /// <param name="obj"></param>
        private void ActivateUIMode(InputAction.CallbackContext obj)
        {
            if (!isUICanvasActive)
            {
                isUICanvasActive = true;

                //Activating UI Controller by enabling its XR Ray Interactor and XR Interactor Line Visual
                uiController.GetComponent<XRRayInteractor>().enabled = true;
                uiController.GetComponent<XRInteractorLineVisual>().enabled = true;

                //Deactivating Base Controller by disabling its XR Direct Interactor
                baseController.GetComponent<XRDirectInteractor>().enabled = false;

                //Adjusting the transform of the UI Canvas Gameobject according to the VR Player transform
                Vector3 positionVec = new Vector3(uiController.transform.position.x, positionOffsetForUICanvasObject.y, uiController.transform.position.z);
                Vector3 directionVec = uiController.transform.forward;
                directionVec.y = 0f;
                uiCanvasObject.transform.position = positionVec + positionOffsetForUICanvasObject.magnitude * directionVec;
                uiCanvasObject.transform.rotation = Quaternion.LookRotation(directionVec);

                //Activating the UI Canvas Gameobject
                uiCanvasObject.SetActive(true);
            }
            else
            {
                isUICanvasActive = false;

                //De-Activating UI Controller by enabling its XR Ray Interactor and XR Interactor Line Visual
                uiController.GetComponent<XRRayInteractor>().enabled = false;
                uiController.GetComponent<XRInteractorLineVisual>().enabled = false;

                //Activating Base Controller by disabling its XR Direct Interactor
                baseController.GetComponent<XRDirectInteractor>().enabled = true;

                //De-Activating the UI Canvas Gameobject
                uiCanvasObject.SetActive(false);
            }

        }
    }
}
