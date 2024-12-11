using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NameTM;

    Player _player;

    void Awake()
    {
        _player = GetComponent<PhotonView>().Owner;
        SetName(_player.NickName, PhotonNetwork.LocalPlayer == _player);
    }

    public void SetName(string setTo, bool isSelf)
    {
        NameTM.text = setTo;
        NameTM.color = isSelf ? Color.green : Color.red;
    }
}
