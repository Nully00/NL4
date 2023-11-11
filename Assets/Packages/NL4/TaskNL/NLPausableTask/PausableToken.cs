using Cysharp.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace NLTask
{
    /// <summary>
    /// �ꎞ��~�\��CancellationToken��񋟂��܂��B
    /// </summary>
    public class PausableToken
    {
        /// <summary>
        /// ���݂̈ꎞ��~��Ԃ��擾���܂��B
        /// </summary>
        public bool IsPaused => IsPausedFlag() || _isPausedToggle;

        /// <summary>
        /// CancellationToken���擾���܂��B
        /// </summary>
        public CancellationToken CancellationToken { get; private set; }

        /// <summary>
        /// CancellationTokenSource���擾���܂��B
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; private set; }

        private readonly Func<bool> IsPausedFlag;
        private bool _isPausedToggle;

        /// <summary>
        /// PausableToken�̐V�����C���X�^���X�����������܂��B
        /// </summary>
        /// <param name="mono">�֘A����MonoBehaviour�B</param>
        /// <param name="isPauseCondition">�ꎞ��~�̏������w�肷��f���Q�[�g�B�ȗ����ꂽ�ꍇ�͏��false��Ԃ��܂��B</param>
        public PausableToken(MonoBehaviour mono, Func<bool> isPauseCondition = null)
        {
            CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                new CancellationTokenSource().Token, mono.GetCancellationTokenOnDestroy());
            CancellationToken = CancellationTokenSource.Token;

            IsPausedFlag = isPauseCondition ?? (() => false);
        }

        /// <summary>
        /// �ꎞ��~���܂��B
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Pause()
        {
            _isPausedToggle = true;
        }

        /// <summary>
        /// �ꎞ��~���������܂��B
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Resume()
        {
            _isPausedToggle = false;
        }

        /// <summary>
        /// �L�����Z����v�����܂��B
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Cancel()
        {
            CancellationTokenSource.Cancel();
        }

        /// <summary>
        /// ���݂�CancellationToken�Ǝw�肳�ꂽCancellationToken��A�����܂��B
        /// </summary>
        /// <param name="cancellationToken">�A������CancellationToken�B</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LinkTokenSource(CancellationToken cancellationToken)
        {
            CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                this.CancellationToken, cancellationToken);
            this.CancellationToken = CancellationTokenSource.Token;
        }
    }
}