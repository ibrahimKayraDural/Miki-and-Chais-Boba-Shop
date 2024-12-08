using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PhotonView @PhotonView { get; private set; }

    void Awake()
    {
        @PhotonView = GetComponent<PhotonView>();
    }
}
