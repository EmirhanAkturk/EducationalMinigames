using EzySlice;
using Minigames.SwordAndPistol.Scripts.Managers;
using UnityEngine;

namespace Minigames.SwordAndPistol.Scripts
{
    public class Slicer : MonoBehaviour
    {
        public bool IsTouched { get; set; }
        
        [SerializeField] private Material materialAfterSlice;
        [SerializeField] private LayerMask sliceMask;

        [Header("Vibration")]
        [SerializeField] private OVRInput.Controller controller = OVRInput.Controller.LTouch;
        [SerializeField] private float duration = .4f;
        [SerializeField] private float frequency = 1f;
        [SerializeField] private float amplitude = .3f;
        
        private void Update()
        {     
            if (IsTouched)
            {
                IsTouched = false;
                Collider[] objectsToBeSliced = Physics.OverlapBox(transform.position, new Vector3(1, 0.1f, 0.1f), transform.rotation, sliceMask);
                foreach (Collider objectToBeSliced in objectsToBeSliced)
                {
                    SlicedHull slicedObject = SliceObject(objectToBeSliced.gameObject, materialAfterSlice);

                    GameObject upperHullGameobject = slicedObject.CreateUpperHull(objectToBeSliced.gameObject, materialAfterSlice);
                    GameObject lowerHullGameobject = slicedObject.CreateLowerHull(objectToBeSliced.gameObject, materialAfterSlice);

                    //Play slice sound
                    AudioManager.Instance.PlaySound(AudioType.SliceSound, objectToBeSliced.transform.position);
                    
                    // Starts vibration
                    VibrationManager.Instance.VibrateController(duration, frequency, amplitude, controller);
             
                    //Add score
                    ScoreManager.Instance.AddScore(ScorePoints.SWORDCUBE_SCOREPOINT);

                    upperHullGameobject.transform.position = objectToBeSliced.transform.position;
                    lowerHullGameobject.transform.position = objectToBeSliced.transform.position;
               
                    MakeItPhysical(upperHullGameobject, objectToBeSliced.gameObject.GetComponent<Rigidbody>().velocity);
                    MakeItPhysical(lowerHullGameobject, objectToBeSliced.gameObject.GetComponent<Rigidbody>().velocity);

                    Destroy(objectToBeSliced.gameObject);
                }
            }

        }
        private void MakeItPhysical(GameObject obj, Vector3 _velocity)
        {
            obj.AddComponent<MeshCollider>().convex = true;
            obj.AddComponent<Rigidbody>();
            obj.GetComponent<Rigidbody>().velocity = -_velocity;

            int randomNumberX = Random.Range(0,2);
            int randomNumberY = Random.Range(0, 2);
            int randomNumberZ = Random.Range(0, 2);

            obj.GetComponent<Rigidbody>().AddForce(3*new Vector3(randomNumberX,randomNumberY,randomNumberZ),ForceMode.Impulse);       
            obj.AddComponent<DestroyAfterSeconds>();

        }

   

        private SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
        {
            // slice the provided object using the transforms of this object
            return obj.Slice(transform.position, transform.up, crossSectionMaterial);
        }

    }
}
