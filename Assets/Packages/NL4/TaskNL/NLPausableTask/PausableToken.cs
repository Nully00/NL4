using Cysharp.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace NLTask
{
    /// <summary>
    /// 一時停止可能なCancellationTokenを提供します。
    /// </summary>
    public class PausableToken
    {
        /// <summary>
        /// 現在の一時停止状態を取得します。
        /// </summary>
        public bool IsPaused => IsPausedFlag() || _isPausedToggle;

        /// <summary>
        /// CancellationTokenを取得します。
        /// </summary>
        public CancellationToken CancellationToken { get; private set; }

        /// <summary>
        /// CancellationTokenSourceを取得します。
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; private set; }

        private readonly Func<bool> IsPausedFlag;
        private bool _isPausedToggle;

        /// <summary>
        /// PausableTokenの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="mono">関連するMonoBehaviour。</param>
        /// <param name="isPauseCondition">一時停止の条件を指定するデリゲート。省略された場合は常にfalseを返します。</param>
        public PausableToken(MonoBehaviour mono, Func<bool> isPauseCondition = null)
        {
            CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                new CancellationTokenSource().Token, mono.GetCancellationTokenOnDestroy());
            CancellationToken = CancellationTokenSource.Token;

            IsPausedFlag = isPauseCondition ?? (() => false);
        }

        /// <summary>
        /// 一時停止します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Pause()
        {
            _isPausedToggle = true;
        }

        /// <summary>
        /// 一時停止を解除します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Resume()
        {
            _isPausedToggle = false;
        }

        /// <summary>
        /// キャンセルを要求します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Cancel()
        {
            CancellationTokenSource.Cancel();
        }

        /// <summary>
        /// 現在のCancellationTokenと指定されたCancellationTokenを連結します。
        /// </summary>
        /// <param name="cancellationToken">連結するCancellationToken。</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LinkTokenSource(CancellationToken cancellationToken)
        {
            CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                this.CancellationToken, cancellationToken);
            this.CancellationToken = CancellationTokenSource.Token;
        }
    }
}