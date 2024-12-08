using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    public int ActorNumber { get; private set; }

    PhotonView _pv;

    void Awake()
    {
        _pv = GetComponent<PhotonView>();
        ActorNumber = _pv.Owner.ActorNumber;
    }

    void Start()
    {
        if (_pv.IsMine)
        {
            Debug.Log("Actor Number: " + ActorNumber);
            CreateController();
        }
    }

    public void CreateController()
    {
        if (!_pv.IsMine)
            return;

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
    }
}
