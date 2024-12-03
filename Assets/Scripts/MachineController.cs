using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemHolder;
using UnityEngine.UI;

public class MachineController : MonoBehaviour
{
    [SerializeField] ItemHolder_MachineOutput Output;
    [SerializeField] List<ItemHolder_MachineInput> Inputs;
    [SerializeField] float _ProcessTime = 2;
    [SerializeField] float _ProgressInterval = .05f;
    [SerializeField] Slider _Slider;

    bool _inProcess;

    private void Awake()
    {
        foreach (var input in Inputs) input.Owner = this;
        Output.Owner = this;

        _Slider.maxValue = _ProcessTime;
        _Slider.value = 0;
    }

    public void CheckStatus()
    {
        if (_inProcess) return;

        bool isReady = true;
        foreach (var i in Inputs)
        {
            if (i.IsReady == false)
            {
                isReady = false;
                break;
            }
        }

        if (Output.IsReady == false) isReady = false;

        if (isReady)
        {
            StartProcess();
        }
    }

    void StartProcess()
    {
        StopCoroutine(nameof(Process));
        StartCoroutine(nameof(Process));
    }

    IEnumerator Process()
    {
        float currentProgress = 0;
        _Slider.maxValue = _ProcessTime;
        _Slider.value = 0;

        _inProcess = true;
        while (true)
        {
            yield return new WaitForSeconds(_ProgressInterval);
            currentProgress += _ProgressInterval;
            _Slider.value = currentProgress;
            if (currentProgress >= _ProcessTime) break;
        }

        OnProcessDone();
    }
    void OnProcessDone()
    {
        foreach (var i in Inputs) i.ResetInput();
        Output.OutputFromMachine();
        _Slider.value = 0;

        _inProcess = false;
    }
}
