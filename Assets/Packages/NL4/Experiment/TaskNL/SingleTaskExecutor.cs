using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// Provides a way to ensure that only one task is executed at a time with a cooldown period after each execution.
/// </summary>
public class SingleTaskExecutor
{
    private bool isTaskCompleted = true;
    private bool isCoolDownCompleted = true;

    private readonly object LockObject = new object();

    public SingleTaskExecutor()
    {
    }
    public bool IsExecuting
    {
        get
        {
            lock (LockObject)
            {
                return !isTaskCompleted || !isCoolDownCompleted;
            }
        }
    }
    /// <summary>
    /// Gets a value indicating whether a task is currently running.
    /// </summary>
    public bool IsTaskRunning
    {
        get
        {
            lock (LockObject)
            {
                return !isTaskCompleted;
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether the executor is in a cooldown period.
    /// </summary>
    public bool IsInCoolDown
    {
        get
        {
            lock (LockObject)
            {
                return !isCoolDownCompleted;
            }
        }
    }

    /// <summary>
    /// Attempts to execute the given task factory.
    /// </summary>
    /// <param name="taskFactory">The task factory to execute.</param>
    /// <returns><c>true</c> if the task factory was executed; otherwise, <c>false</c>.</returns>
    public bool TryExecute(Func<UniTask> taskFactory/*,coolDownDuration,PausableToken*/)
    {
        lock (LockObject)
        {
            if (IsExecuting)
            {
                return false;
            }

            _ = ExecuteInternal(taskFactory);
            return true;
        }
    }

    private async UniTask ExecuteInternal(Func<UniTask> taskFactory)
    {
        await ExecuteTask(taskFactory);
        await ExecuteCooldown();
    }

    private async UniTask ExecuteTask(Func<UniTask> taskFactory)
    {
        SetTaskRunningFlag(true);
        try
        {
            await taskFactory();
        }
        finally
        {
            SetTaskRunningFlag(false);
        }
    }

    private async UniTask ExecuteCooldown()
    {
        SetCooldownFlag(true);
        try
        {
            // ここでのPausableTask.WaitForSecondsはコメントアウトされていたので、
            // 必要に応じてアンコメントしてください。
            // await PausableTask.WaitForSeconds(cooldowntime);
        }
        finally
        {
            SetCooldownFlag(false);
        }
    }

    private void SetTaskRunningFlag(bool isRunning)
    {
        lock (LockObject)
        {
            isTaskCompleted = !isRunning;
        }
    }

    private void SetCooldownFlag(bool isInCooldown)
    {
        lock (LockObject)
        {
            isCoolDownCompleted = !isInCooldown;
        }
    }
}