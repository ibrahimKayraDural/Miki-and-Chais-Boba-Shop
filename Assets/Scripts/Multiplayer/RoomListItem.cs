using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI RoomName;
    [SerializeField] TextMeshProUGUI _PlayerCount;

    public RoomInfo info;

    public void Setup(RoomInfo _info)
    {
        info = _info;
        RoomName.text = _info.Name;
        _PlayerCount.text = _info.PlayerCount + "/" + _info.MaxPlayers;
        _PlayerCount.color = _info.PlayerCount < _info.MaxPlayers ? Color.white : Color.red;
    }

    public void JoinSelectedRoom()
    {
        Launcher.Instance.JoinRoom(info);
    }

}
