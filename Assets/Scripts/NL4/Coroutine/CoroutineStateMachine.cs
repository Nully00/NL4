using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NL4.Coroutine
{
    /// <summary>
    /// コルーチンのステートとそれに関連するイベントを管理するクラスです。
    /// Manages coroutine states and their associated events.
    /// </summary>
    public class CoroutineStateMachine<TState>
    {
        public TState currentState { get; set; }
        private StateEvent _currentEvent;

        public bool isDebugLog { get; set; } = false;

        private CoroutineController _controller;
        private Dictionary<TState, StateEvent> _stateToEventMap = new Dictionary<TState, StateEvent>();

        private bool _isChangingState = false;

        private UnityEngine.Coroutine _update = null;
        /// <summary>
        /// CoroutineControllerを指定して新しいインスタンスを初期化します。
        /// Initializes a new instance with the specified CoroutineController.
        /// </summary>
        /// <param name="controller">コルーチンの制御を行うコントローラー。The controller to manage the coroutine.</param>
        public CoroutineStateMachine(CoroutineController controller)
        {
            _controller = controller;
        }
        /// <summary>
        /// ステートと関連するイベントを追加します。
        /// Adds a state with its associated events.
        /// </summary>
        /// <param name="state">追加するステート。The state to add.</param>
        /// <param name="onEnterCoroutine">ステートが開始されたときのコルーチン。Coroutine to run when the state starts.</param>
        /// <param name="onEnter">ステートが開始されたときのアクション。Action to run when the state starts.</param>
        /// <param name="onExit">ステートが終了したときのアクション。Action to run when the state ends.</param>
        /// <param name="onUpdate">ステートが更新されるたびのアクション。Action to run on state update.</param>
        public void AddState(TState state, Func<IEnumerator> onEnterCoroutine, Action onEnter, Action onExit, Action onUpdate)
        {
            _stateToEventMap.Add(state, new StateEvent(onEnterCoroutine, onEnter, onExit, onUpdate));
        }
        /// <summary>
        /// ステートと関連するイベントを追加します。
        /// Adds a state with its associated events.
        /// </summary>
        public void AddState(TState state, Func<IEnumerator> onEnterCoroutine)
        {
            _stateToEventMap.Add(state, new StateEvent(onEnterCoroutine, null, null, null));
        }
        /// <summary>
        /// ステートと関連するイベントを追加します。
        /// Adds a state with its associated events.
        /// </summary>
        public void AddState(TState state, Action onEnter, Action onExit, Action onUpdate)
        {
            _stateToEventMap.Add(state, new StateEvent(null, onEnter, onExit, onUpdate));
        }
        /// <summary>
        /// 指定されたステートでコルーチンを開始します。
        /// Starts the coroutine with the specified state.
        /// </summary>
        /// <param name="currentState">開始するステート。The state to start.</param>
        public void Start(TState currentState)
        {
            DebugLog($"State：{currentState}");
            this.currentState = currentState;
            _isChangingState = false;

            if (!_stateToEventMap.TryGetValue(currentState, out _currentEvent))
                throw new ArgumentException("指定されたStateは登録されていません。");


            if (_currentEvent.onEnterCoroutine != null)
            {
                _controller.Run(_currentEvent.onEnterCoroutine());
            }
            _currentEvent.onEnter?.Invoke();

            if (_update == null)
                _update = _controller.mono.StartCoroutine(Update());
        }
        /// <summary>
        /// 指定された次のステートに変更します。
        /// Changes to the specified next state.
        /// </summary>
        /// <param name="nextState">次のステート。The next state.</param>
        /// <returns>ステート変更のコルーチン。Coroutine for state change.</returns>
        public IEnumerator ChangeState(TState nextState)
        {
            _isChangingState = true;
            currentState = nextState;
            yield return _controller.KillFromInside();
        }
        /// <summary>
        /// コルーチンを終了します。
        /// Kills the coroutine.
        /// </summary>
        public void Kill()
        {
            _controller.Kill();
            _controller.mono.StopCoroutine(_update);
            _update = null;
        }
        /// <summary>
        /// コルーチンを一時停止します。
        /// Pauses the coroutine.
        public void Pause()
        {
            _controller.Pause();
        }
        /// <summary>
        /// コルーチンを再開します。
        /// Restarts the coroutine.
        /// </summary>
        public void Restart()
        {
            _controller.Restart();
        }
        private IEnumerator Update()
        {
            bool pauseLog = false;
            while (true)
            {
                if (_controller.isPausing)
                {
                    if (!pauseLog)
                        DebugLog($"Pause");
                    continue;
                }

                if (_isChangingState)
                {
                    _currentEvent.onExit?.Invoke();
                    Start(currentState);
                }

                _currentEvent.onUpdate?.Invoke();

                pauseLog = false;
                yield return null;
            }
        }
        private void DebugLog(string log)
        {
#if UNITY_EDITOR
            if (!isDebugLog) return;

            Debug.Log(log);
#endif
        }
    }
    public class StateEvent
    {
        public Func<IEnumerator> onEnterCoroutine { get; private set; } = null;
        public Action onEnter { get; private set; } = null;
        public Action onExit { get; private set; } = null;
        public Action onUpdate { get; private set; } = null;

        public StateEvent(Func<IEnumerator> onEnterCoroutine, Action onEnter, Action onExit, Action onUpdate)
        {
            this.onEnterCoroutine = onEnterCoroutine;
            this.onEnter = onEnter;
            this.onExit = onExit;
            this.onUpdate = onUpdate;
        }
    }
}