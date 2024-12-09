using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PhotonView @PhotonView { get; private set; }

    [SerializeField] PlayerMovementController _PlayerMovementController;
    [SerializeField] PlayerInteractor _PlayerInteractor;
    [SerializeField] PlayerCamera _PlayerCamera;

    void Awake()
    {
        @PhotonView = GetComponent<PhotonView>();

        if (@PhotonView.IsMine == false)
        {
            _PlayerMovementController.Deactivate();
            _PlayerInteractor.Deactivate();
            _PlayerCamera.Deactivate();
        }
    }
}
