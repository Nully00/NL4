using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineController
{
    public MonoBehaviour mono { get; private set; }
    public bool isPausing { get; private set; } = false;

    private EfficientStackedStorage<IEnumerator> _linkedCoroutines = new EfficientStackedStorage<IEnumerator>();

    public CoroutineController(MonoBehaviour mono)
    {
        this.mono = mono;
    }

    public void Run(IEnumerator enumerator)
    {
        mono.StartCoroutine(Register(enumerator));
    }
    public IEnumerator WaitRun(IEnumerator enumerator)
    {
        yield return mono.StartCoroutine(Register(enumerator));
    }
    public void RunChild(IEnumerator enumerator)
    {
        mono.StartCoroutine(Register(enumerator));
    }
    public IEnumerator WaitRunChild(IEnumerator enumerator)
    {
        yield return Register(enumerator);
    }

    public IEnumerator Register(IEnumerator enumerator)
    {
        var id = _linkedCoroutines.Deposit(enumerator);
        yield return enumerator;
        _linkedCoroutines.Release(id);
    }

    
    public void Kill()
    {
        foreach (var linkCoroutine in _linkedCoroutines)
        {
            mono.StopCoroutine(linkCoroutine);
            var temp = linkCoroutine;
            temp = null;
        }
        _linkedCoroutines.AllRelease();
    }

    public void Pause()
    {
        isPausing = true;
    }

    public void Restart()
    {
        isPausing = false;
    }


    /// <summary>
    /// フレーム待機を行います。
    /// Waits for a frame.
    /// </summary>
    public IEnumerator WaitForFrame()
    {
        do
        {
            yield return null;
        }
        while (isPausing);
    }

    /// <summary>
    /// 指定時間待機を行います。
    /// Waits for a specified time.
    /// </summary>
    /// <param name="seconds">待機する秒数(Number of seconds to wait)</param>
    public IEnumerator WaitForSeconds(float seconds)
    {
        float totalTime = 0;
        while (totalTime < seconds)
        {
            yield return WaitForFrame();
            totalTime += Time.deltaTime;
        }
    }
}
