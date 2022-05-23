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

    [SerializeField] private GameObject[] AvatarModelPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            //The player is local
            LocalXRRigGameobject.SetActive(true);

            //Getting the avatar selection data so that the correct avatar model can be instantiated.
            object avatarSelectionNumber;
            if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerVRConstants.AVATAR_SELECTION_NUMBER, out avatarSelectionNumber))
            {
                Debug.Log("Avatar selection number: " + (int)avatarSelectionNumber);
                photonView.RPC("InitializeSelectedAvatarModel", RpcTarget.AllBuffered, (int)avatarSelectionNumber);
            }

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

    [PunRPC]
    public void InitializeSelectedAvatarModel(int avatarSelectionNumber)
    {
        GameObject selectedAvatarGameobject = Instantiate(AvatarModelPrefabs[avatarSelectionNumber], LocalXRRigGameobject.transform);

        AvatarInputConverter avatarInputConverter = LocalXRRigGameobject.GetComponent<AvatarInputConverter>();
        AvatarHolder avatarHolder = selectedAvatarGameobject.GetComponent<AvatarHolder>();
        SetUpAvatarGameobject(avatarHolder.HeadTransform, avatarInputConverter.AvatarHead);
        SetUpAvatarGameobject(avatarHolder.BodyTransform, avatarInputConverter.AvatarBody);
        SetUpAvatarGameobject(avatarHolder.HandLeftTransform, avatarInputConverter.AvatarHand_Left);
        SetUpAvatarGameobject(avatarHolder.HandRightTransform, avatarInputConverter.AvatarHand_Right);
    }

    void SetUpAvatarGameobject(Transform avatarModelTransform, Transform mainAvatarTransform)
    {
        avatarModelTransform.SetParent(mainAvatarTransform);
        avatarModelTransform.localPosition = Vector3.zero;
        avatarModelTransform.localRotation = Quaternion.identity;
    }
}
