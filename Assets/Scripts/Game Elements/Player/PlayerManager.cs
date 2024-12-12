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

        var spawn = FindObjectOfType<SpawnPoint>();
        Vector2 spawnPoint = spawn == null ? Vector2.zero : spawn.transform.position;
        var go = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), spawnPoint, Quaternion.identity);
    }
}
