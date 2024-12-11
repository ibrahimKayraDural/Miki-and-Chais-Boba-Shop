using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuInteractor : MonoBehaviour
{
    PauseMenuManager _pauseMenuManager
    {
        get
        {
            if (AUTO_pauseMenuManager == null)
                AUTO_pauseMenuManager = PauseMenuManager.Instance;
            return AUTO_pauseMenuManager;
        }
    }
    PauseMenuManager AUTO_pauseMenuManager = null;

    void Awake()
    {
        Player p = GetComponent<PhotonView>().Owner;
        if (p != PhotonNetwork.LocalPlayer) enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("PauseGame")) _pauseMenuManager?.ToggleMenu();
    }
}
