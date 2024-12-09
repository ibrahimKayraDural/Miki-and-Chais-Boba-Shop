using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; } = null;

    public Vector2 CameraBounds => _CameraBounds;
    public Vector2 CameraBoundsOffset => _CameraBoundsOffset;

    [Header("Camera Bounds")]
    [SerializeField] Vector2 _CameraBounds = new Vector2(16, 9);
    [SerializeField] Vector2 _CameraBoundsOffset = Vector2.zero;
    [Header("Reference")]
    [SerializeField] TextMeshProUGUI _MoneyTM;

    PhotonView _photonView;
    int _todaysMoney = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);

        _photonView = GetComponent<PhotonView>();
    }

    public void AddMoney(int amount) => SetMoney(_todaysMoney + amount);
    public void RemoveMoney(int amount) => SetMoney(_todaysMoney - amount);
    public void SetMoney(int setTo)
    {
        _todaysMoney = Mathf.Clamp(setTo, 0, GV.MaxMoneyPerDay);
        _MoneyTM.text = _todaysMoney.ToString();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_CameraBoundsOffset, _CameraBounds);
    }
}
