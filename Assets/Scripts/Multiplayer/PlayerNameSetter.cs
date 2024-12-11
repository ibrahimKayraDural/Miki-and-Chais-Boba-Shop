using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerNameSetter : MonoBehaviour
{
    [SerializeField] TMP_InputField _NameField;

    public void RefreshNickname()
    {
        _NameField.text = PhotonNetwork.NickName;
    }

    public void OnEndEdit()
    {
        PhotonNetwork.NickName = _NameField.text;
    }
}
