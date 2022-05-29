using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkingGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    public bool IsBeingHeld { get; private set; } = false;
    
    private PhotonView m_photonView;
    private XRGrabInteractable xrGrabInteractable;
    private Rigidbody rb;
    
    
    private void Awake()
    {
        m_photonView = GetComponent<PhotonView>();
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBeingHeld)
        {
            //Object is being grabbed
            rb.isKinematic = true;
        }
        else
        {
            //Object is not being grabbed
            rb.isKinematic = false;
        }
    }

    private void TransferOwnerShip()
    {
        m_photonView.RequestOwnership();
    }

    public void OnSelectEntered()
    {
        Debug.Log("Grabbed");
        m_photonView.RPC("StartNetworkGrabbing", RpcTarget.AllBuffered);

        if(m_photonView.Owner == PhotonNetwork.LocalPlayer)
        {
            Debug.Log("We do not request the ownership. Already mine.");
        }
        else
        {
            TransferOwnerShip();
        }
    }

    public void OnSelectExited()
    {
        Debug.Log("Released");
        m_photonView.RPC("StopNetworkGrabbing", RpcTarget.AllBuffered);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if(targetView != m_photonView)
        {
            return;
        }

        Debug.Log("Ownership Requested for: " + targetView.name + " from " + requestingPlayer.NickName);
        m_photonView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("OnOwnership transfered to " + targetView.name + " from " + previousOwner.NickName);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {

    }

    [PunRPC]
    public void StartNetworkGrabbing()
    {
        //InHand Layer
        IsBeingHeld = true;
        // xrGrabInteractable.interactionLayers = InteractionLayerMask.GetMask("InHand");
        gameObject.layer = LayerMask.NameToLayer("InHand");
    }

    [PunRPC]
    public void StopNetworkGrabbing()
    {
        //Interactable layer
        IsBeingHeld = false;
        // xrGrabInteractable.interactionLayers = InteractionLayerMask.GetMask("Interactable");
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }
}
