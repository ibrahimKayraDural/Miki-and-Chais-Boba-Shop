using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; } = null;

    [SerializeField] TextMeshProUGUI _MoneyTM;

    int _todaysMoney = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
    }

    public void AddMoney(int amount) => SetMoney(_todaysMoney + amount);
    public void RemoveMoney(int amount) => SetMoney(_todaysMoney - amount);
    public void SetMoney(int setTo)
    {
        _todaysMoney = Mathf.Clamp(setTo, 0, GLOBALVALUES.MaxMoneyPerDay);
        _MoneyTM.text = _todaysMoney.ToString();
    }
}
