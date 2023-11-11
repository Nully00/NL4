using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
namespace NLTask
{
    public static class PausableTask
    {
        public static async UniTask Yield(PausableToken token)
        {
            await Yield(PlayerLoopTiming.TimeUpdate, token);
        }
        public static async UniTask Yield(PlayerLoopTiming timing, PausableToken token)
        {
            NullChecker.ThrowIfNull(token,nameof(token));
            await YieldWithoutNullCheck(timing, token);
        }

        public static async UniTask YieldWithoutNullCheck(PlayerLoopTiming timing, PausableToken token)
        {
            do
            {
                await UniTask.Yield(timing, token.CancellationToken);
            }
            while (token.IsPaused);
        }
        public static async UniTask WaitForSeconds(float seconds, PausableToken token)
        {
            token.CancellationToken.ThrowIfCancellationRequested();
            NullChecker.ThrowIfNull(token, nameof(token));
            float totalTime = 0;
            while (totalTime < seconds)
            {
                await YieldWithoutNullCheck(PlayerLoopTiming.TimeUpdate, token);
                totalTime += Time.deltaTime;
            }
        }


        public static async UniTask WaitThenInvoke(float seconds, Action action, PausableToken token)
        {
            await WaitForSeconds(seconds, token);
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while executing the action.", ex);
            }
        }

        public static async UniTask WaitTaskThenInvoke(UniTask task, Action action)
        {
            await task;
            action?.Invoke();
        }
        public static async UniTask ChainTasks(params UniTask[] task)
        {
            for (int i = 0; i < task.Length; i++)
            {
                await task[i];
            }
        }
        public static async UniTask WaitUntil(Func<bool> condition, PausableToken token)
        {
            while (!condition())
            {
                await Yield(token);
            }
        }
        public static async UniTask WaitUntil(Func<bool> condition, UniTask task, PausableToken token = null)
        {
            await WaitUntil(condition, token);
            await task;
        }
        public static async UniTask WaitWhile(Func<bool> condition, PausableToken token)
        {
            while (condition())
            {
                await Yield(token);
            }
        }
        public static async UniTask WaitWhile(Func<bool> condition, UniTask task, PausableToken token = null)
        {
            await WaitWhile(condition, token);
            await task;
        }
        public static async UniTask WhenAll(params UniTask[] tasks)
        {
            await UniTask.WhenAll(tasks);
        }


        public static async UniTask ActionWhileDuration(float seconds, Action action, PausableToken token)
        {
            await ActionWhileDuration_Internal(seconds, t => action(), token);
        }
        public static async UniTask ActionWhileDuration(float seconds, Action<float> action, PausableToken token)
        {
            await ActionWhileDuration_Internal(seconds, action, token);
        }
        public static async UniTask ActionWhileDuration01(float seconds, Action<float> action, PausableToken token)
        {
            await ActionWhileDuration_Internal(seconds, t => action(t / seconds), token);
        }
        private static async UniTask ActionWhileDuration_Internal(float seconds, Action<float> action, PausableToken token)
        {
            float totalTime = 0;
            action(0);
            while (totalTime < seconds)
            {
                await Yield(token);
                totalTime += UnityEngine.Time.deltaTime;
                action(Mathf.Min(totalTime, seconds));
            }
        }
        public static async UniTask ActionWhileTask(UniTask task, Action action, PausableToken token)
        {
            bool finished = false;
            _ = WaitTaskThenInvoke(task, () => finished = true);
            while (!finished)
            {
                action();
                await Yield(token);
            }
        }

        public static class NullChecker
        {
            /// <summary>
            /// 指定されたオブジェクトがnullである場合、ArgumentNullExceptionをスローします。
            /// </summary>
            /// <param name="value">チェックするオブジェクト。</param>
            /// <param name="paramName">オブジェクトの名前。例外メッセージで使用されます。</param>
            public static void ThrowIfNull(object value, string paramName)
            {
                if (value == null)
                {
                    throw new ArgumentNullException(paramName);
                }
            }
        }
    }
}