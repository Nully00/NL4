using System;
using System.Collections;
using UnityEngine;
/// <summary>
/// コルーチンの制御を行うクラスです。
/// A class for controlling coroutines.
/// </summary>
public class CoroutineController
{
    public MonoBehaviour mono { get; private set; }
    public bool isPausing { get; private set; } = false;

    private EfficientStackedStorage<IEnumerator> _linkedCoroutines = new EfficientStackedStorage<IEnumerator>();

    public event Action OnRun;
    public event Action OnKill;
    public event Action OnPause;
    public event Action OnRestart;
    /// <summary>
    /// MonoBehaviourを指定して新しいインスタンスを初期化します。
    /// Initializes a new instance with the specified MonoBehaviour.
    /// </summary>
    /// <param name="mono">コルーチンの実行に使用するMonoBehaviour。</param>
    public CoroutineController(MonoBehaviour mono)
    {
        if(mono == null)
        {
            throw new NullReferenceException("MonoBehaviour is null");
        }
        this.mono = mono;
    }
    /// <summary>
    /// コルーチンを実行します。
    /// Executes the coroutine.
    /// </summary>
    /// <param name="enumerator">実行するコルーチン。</param>
    public void Run(IEnumerator enumerator)
    {
        OnRun?.Invoke();
        mono.StartCoroutine(Register(enumerator, null, null));
    }
    /// <summary>
    /// コルーチンを実行します。
    /// Executes the coroutine.
    /// </summary>
    /// <param name="enumerator">実行するコルーチン。</param>
    public void Run(IEnumerator enumerator, Action<Exception> onError, Action onComplete)
    {
        OnRun?.Invoke();
        mono.StartCoroutine(Register(enumerator,onError, onComplete));
    }
    /// <summary>
    /// コルーチンを実行します。
    /// Executes the coroutine.
    /// </summary>
    /// <param name="enumerator">実行するコルーチン。</param>
    public void Run(IEnumerator enumerator, Action onComplete)
    {
        OnRun?.Invoke();
        mono.StartCoroutine(Register(enumerator, null, onComplete));
    }
    /// <summary>
    /// コルーチンを実行します。(待機可能)
    /// Executes the coroutine.
    /// </summary>
    /// <param name="enumerator">実行するコルーチン。</param>
    public IEnumerator WaitRun(IEnumerator enumerator, Action<Exception> onError = null)
    {
        OnRun?.Invoke();
        yield return mono.StartCoroutine(Register(enumerator, onError));
    }
    /// <summary>
    /// コルーチンを実行します。(待機可能)
    /// Executes the coroutine.
    /// </summary>
    /// <param name="enumerator">実行するコルーチン。</param>
    public IEnumerator WaitRun(params IEnumerator[] enumerator)
    {
        int finishCount = 0;
        for (int i = 0; i < enumerator.Length; i++)
        {
            Run(RegisterCallBack(enumerator[i], () => finishCount++));
        }

        yield return WaitUntil(()=> finishCount == enumerator.Length);
    }
    /// <summary>
    /// コルーチンを実行します。
    /// Executes the coroutine.
    /// </summary>
    /// <param name="enumerator">実行するコルーチン。</param>
    public void RunChild(IEnumerator enumerator)
    {
        OnRun?.Invoke();
        mono.StartCoroutine(Register(enumerator, null, null));
    }
    /// <summary>
    /// コルーチンを実行します。
    /// Executes the coroutine.
    /// </summary>
    /// <param name="enumerator">実行するコルーチン。</param>
    public void RunChild(IEnumerator enumerator, Action<Exception> onError, Action onComplete)
    {
        OnRun?.Invoke();
        mono.StartCoroutine(Register(enumerator, onError, onComplete));
    }
    /// <summary>
    /// コルーチンを実行します。
    /// Executes the coroutine.
    /// </summary>
    /// <param name="enumerator">実行するコルーチン。</param>
    public void RunChild(IEnumerator enumerator, Action onComplete)
    {
        OnRun?.Invoke();
        mono.StartCoroutine(Register(enumerator, null, onComplete));
    }
    /// <summary>
    /// コルーチンを実行します。(待機可能)
    /// Executes the coroutine.
    /// </summary>
    /// <param name="enumerator">実行するコルーチン。</param>
    public IEnumerator WaitRunChild(IEnumerator enumerator, Action<Exception> onError = null)
    {
        OnRun?.Invoke();
        yield return Register(enumerator, onError);
    }
    /// <summary>
    /// コルーチンを実行します。(待機可能)
    /// Executes the coroutine.
    /// </summary>
    /// <param name="enumerator">実行するコルーチン。</param>
    public IEnumerator WaitRunChild(params IEnumerator[] enumerator)
    {
        yield return WaitRun(enumerator);
    }
    /// <summary>
    /// すべてのコルーチンを停止します。
    /// Stops all the coroutines.
    /// </summary>
    public void Kill()
    {
        foreach (var linkCoroutine in _linkedCoroutines)
        {
            mono.StopCoroutine(linkCoroutine);
            var temp = linkCoroutine;
            temp = null;
        }
        _linkedCoroutines.AllRelease();
        OnKill?.Invoke();
    }
    /// <summary>
    /// 内部からすべてのコルーチンを停止します。
    /// Stops all the coroutines from inside.
    /// </summary>
    public IEnumerator KillFromInside()
    {
        Kill();
        yield return WaitForFrame();
    }
    /// <summary>
    /// コルーチンの実行を一時停止します。
    /// Pauses the execution of coroutines.
    /// </summary>
    public void Pause()
    {
        isPausing = true;
        OnPause?.Invoke();
    }
    /// <summary>
    /// コルーチンの実行を再開します。
    /// Resumes the execution of coroutines.
    /// </summary>
    public void Restart()
    {
        isPausing = false;
        OnRestart?.Invoke();
    }
    /// <summary>
    /// コルーチンを登録します。
    /// Registers the coroutine.
    /// </summary>
    /// <param name="enumerator">登録するコルーチン。</param>
    public IEnumerator Register(IEnumerator enumerator, Action<Exception> onError = null, Action onComplete = null)
    {
        var coroutine = CreateErrorHandlingCoroutine(enumerator, onError, onComplete);
        var id = _linkedCoroutines.Deposit(coroutine);
        yield return coroutine;
        _linkedCoroutines.Release(id);
    }

    private IEnumerator CreateErrorHandlingCoroutine(IEnumerator coroutine,Action<Exception> onError,Action onComplete)
    {
        while (true)
        {
            object current = null;
            Exception ex = null;
            try
            {
                if (!coroutine.MoveNext())
                {
                    break;
                }
                current = coroutine.Current;
            }
            catch (Exception e)
            {
                ex = e;
                onError?.Invoke(e);
            }

            if (ex != null)
            {
                yield return ex;
                yield break;
            }
            yield return current;
        }
        onComplete?.Invoke();
    }
    #region Common
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
    #endregion
    #region DelayAction
    /// 指定時間待った後、指定されたアクションを実行します。
    /// Executes the specified action after waiting for the given duration.
    /// </summary>
    public void DelayAction(float seconds, Action action)
    {
        Run(DelayAction_Internal(seconds, action));
    }
    internal IEnumerator DelayAction_Internal(float seconds, Action action)
    {
        yield return WaitForSeconds(seconds);
        action?.Invoke();
    }
    #endregion
    #region Until,While
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
    #endregion
    #region RunWhen
    /// <summary>
    /// 条件が真の場合にコルーチンを実行します。
    /// Executes coroutines when the condition is true.
    /// </summary>
    /// <param name="conditionalCoroutines">条件とコルーチンのペアのリスト。</param>
    public IEnumerator WaitRunWhenTrue(params (Func<bool> condition,IEnumerator enumerator)[] conditionalCoroutines)
    {
        yield return WaitRun(RunWhen_Internal(true, conditionalCoroutines));
    }
    /// <summary>
    /// 条件が真の場合にコルーチンを実行します。
    /// Executes coroutines when the condition is true.
    /// </summary>
    /// <param name="conditionalCoroutines">条件とコルーチンのペアのリスト。</param>
    public void RunWhenTrue(params (Func<bool> condition,IEnumerator enumerator)[] conditionalCoroutines)
    {
        Run(RunWhen_Internal(true, conditionalCoroutines));
    }
    /// <summary>
    /// 条件が偽の場合にコルーチンを実行します。
    /// Executes coroutines when the condition is false.
    /// </summary>
    /// <param name="conditionalCoroutines">条件とコルーチンのペアのリスト。</param>
    public IEnumerator WaitRunWhenFalse(params (Func<bool> condition, IEnumerator enumerator)[] conditionalCoroutines)
    {
        yield return WaitRun(RunWhen_Internal(false,conditionalCoroutines));
    }
    /// <summary>
    /// 条件が偽の場合にコルーチンを実行します。
    /// Executes coroutines when the condition is false.
    /// </summary>
    /// <param name="conditionalCoroutines">条件とコルーチンのペアのリスト。</param>
    public void RunWhenFalse(params (Func<bool> condition, IEnumerator enumerator)[] conditionalCoroutines)
    {
        Run(RunWhen_Internal(false,conditionalCoroutines));
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
    #endregion
    #region OverTimeAction
    /// <summary>
    /// 指定時間かけてアクションを実行します。
    /// </summary>
    public IEnumerator WaitOverTimeAction(float seconds, Action action)
    {
        yield return WaitRun(OverTimeAction_Internal(seconds,x => action()));
    }
    /// <summary>
    /// 指定時間かけてアクションを実行します。
    /// </summary>
    public void OverTimeAction(float seconds, Action action)
    {
        WaitRun(OverTimeAction_Internal(seconds,x => action()));
    }
    /// <summary>
    /// 指定時間かけてアクションを実行します。
    /// Actionの引数に現在の経過時間が入ります。
    /// </summary>
    public IEnumerator WaitOverTimeAction(float seconds, Action<float> action)
    {
        yield return WaitRun(OverTimeAction_Internal(seconds, action));
    }
    /// <summary>
    /// 指定時間かけてアクションを実行します。
    /// Actionの引数に現在の経過時間が入ります。
    /// </summary>
    public void OverTimeAction(float seconds, Action<float> action)
    {
        WaitRun(OverTimeAction_Internal(seconds, action));
    }
    /// <summary>
    /// 指定時間かけてアクションを実行します。
    /// Actionの引数に現在の経過時間(0-1)が入ります。
    /// </summary>
    public IEnumerator WaitOverTimeAction01(float seconds, Action<float> action)
    {
        yield return WaitRun(OverTimeAction_Internal(seconds, x => action(x / seconds)));
    }
    /// <summary>
    /// 指定時間かけてアクションを実行します。
    /// Actionの引数に現在の経過時間(0-1)が入ります。
    /// </summary>
    public void OverTimeAction01(float seconds, Action<float> action)
    {
        WaitRun(OverTimeAction_Internal(seconds, x => action(x / seconds)));
    }
    private IEnumerator OverTimeAction_Internal(float seconds, Action<float> action)
    {
        float totalTime = 0;
        action(0);
        while (totalTime < seconds)
        {
            yield return WaitForFrame();
            totalTime += UnityEngine.Time.deltaTime;
            action(Mathf.Min(totalTime, seconds));
        }
    }
    #endregion
    #region Other
    internal IEnumerator RegisterCallBack(IEnumerator enumerator, Action action)
    {
        yield return Register(enumerator);
        action();
    }
    #endregion
}