using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
public class PlayerNetworkSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject LocalXRRigGameobject;

    [SerializeField] private GameObject AvatarHeadGameobject;
    [SerializeField] private GameObject AvatarBodyGameobject;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            //The player is local
            LocalXRRigGameobject.SetActive(true);

            SetLayerRecursively(AvatarHeadGameobject, 10);
            SetLayerRecursively(AvatarBodyGameobject, 11);

            TeleportationArea[] teleportationAreas = FindObjectsOfType<TeleportationArea>();
            if(teleportationAreas.Length > 0)
            {
                Debug.Log("Found " + teleportationAreas.Length + "teleportation area. ");
                foreach(var item in teleportationAreas)
                {
                    item.teleportationProvider = LocalXRRigGameobject.GetComponent<TeleportationProvider>();
                }
            }
        }
        else
        {
            //The player is remote
            LocalXRRigGameobject.SetActive(false);

            SetLayerRecursively(AvatarHeadGameobject, 0);
            SetLayerRecursively(AvatarBodyGameobject, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}
