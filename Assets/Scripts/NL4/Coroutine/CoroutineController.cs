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
    public IEnumerator WaitRun(params IEnumerator[] enumerator)
    {
        int finishCount = 0;
        for (int i = 0; i < enumerator.Length; i++)
        {
            Run(RegisterCallBack(enumerator[i], () => finishCount++));
        }

        yield return WaitUntil(()=> finishCount == enumerator.Length);
    }
    public void RunChild(IEnumerator enumerator)
    {
        mono.StartCoroutine(Register(enumerator));
    }
    public IEnumerator WaitRunChild(IEnumerator enumerator)
    {
        yield return Register(enumerator);
    }
    public IEnumerator WaitRunChild(params IEnumerator[] enumerator)
    {
        yield return WaitRun(enumerator);
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
    public IEnumerator KillFromInside()
    {
        Kill();
        yield return WaitForFrame();
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

    internal IEnumerator RegisterCallBack(IEnumerator enumerator,Action action)
    {
        yield return Register(enumerator);
        action();
    }

    /// <summary>
    /// 指定した条件が満たされるまで待ちます。
    /// Waits until the specified condition is met.
    /// </summary>
    /// <param name="condition">待機する条件(Condition to wait for)</param>
    public IEnumerator WaitUntil(Func<bool> condition)
    {

        while (!condition())
        {
            yield return WaitForFrame();
        }
    }
    /// <summary>
    /// 指定した条件が満たされなくなるまで待ちます。
    /// Waits until the specified condition is no longer met.
    /// </summary>
    /// <param name="condition">待機する条件(Condition to wait for)</param>
    public IEnumerator WaitWhile(Func<bool> condition)
    {
        while (condition())
        {
            yield return WaitForFrame();
        }
    }

    public IEnumerator RunWhenTrue(params (Func<bool> condition,IEnumerator enumerator)[] conditionalCoroutines)
    {
        yield return RunWhen_Internal(true, conditionalCoroutines);
    }
    public IEnumerator RunWhenFalse(params (Func<bool> condition, IEnumerator enumerator)[] conditionalCoroutines)
    {
        yield return RunWhen_Internal(false,conditionalCoroutines);
    }
    internal IEnumerator RunWhen_Internal(bool flag,params (Func<bool> condition, IEnumerator enumerator)[] conditionalCoroutines)
    {
        EfficientHidingArray<(Func<bool> condition, IEnumerator enumerator)> conditionalCoroutine_HidingArray =
            new EfficientHidingArray<(Func<bool>, IEnumerator)>(conditionalCoroutines);
        int finishCount = 0;

        while (conditionalCoroutine_HidingArray.Length != 0)
        {
            for (int i = 0; i < conditionalCoroutine_HidingArray.Length; i++)
            {
                var temp = conditionalCoroutine_HidingArray[i];
                if (!(flag && temp.condition()))
                    continue;

                Run(RegisterCallBack(temp.enumerator, () => finishCount++));
                conditionalCoroutine_HidingArray.ReserveHideAtIndex(i);
            }
            conditionalCoroutine_HidingArray.Hide();
            yield return WaitForFrame();
        }
        yield return WaitUntil(() => finishCount == conditionalCoroutines.Length);
    }
}


public static class ListExtensions
{
    public static void RemoveAtEfficiently<T>(this List<T> list, int index)
    {
        if ((uint)index >= (uint)list.Count)
        {
            ThrowHelper.ArgumentOutOfRangeException();
        }
        int lastIdx = list.Count - 1;

        (list[index], list[lastIdx]) = (list[lastIdx], list[index]);
        list.RemoveAt(lastIdx);
    }
}

public static class GenerateArray
{
    public static int[] Range(int length)
    {
        int[] result = new int[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = i;
        }
        return result;
    }
}
public static class ThrowHelper
{
    public static void ArgumentOutOfRangeException()
    {
        throw new ArgumentOutOfRangeException();
    }
}
