using System;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;
using System.Linq;
using NUnit.Framework;
using NL4.Coroutine;

public class CoroutineMachineTest
{
    [UnityTest]
    public IEnumerator MachineTest()
    {
        yield return new MonoBehaviourTest<CoroutineControllerMonoBehaviourTest>();
    }
    public class CoroutineControllerMonoBehaviourTest : MonoBehaviour, IMonoBehaviourTest
    {
        private bool _finished = false;
        public bool IsTestFinished => _finished;
        public enum State
        {
            A,
            B,
            C,
            D,
            E,
        }
        private IEnumerator Start()
        {
            Func<Action, IEnumerator>[] coroutines =
            {
                StartTest,StackOverFlowTest,KillTest
            };
            bool[] finished = new bool[coroutines.Length];

            for (int i = 0; i < finished.Length; i++)
            {
                finished[i] = false;
                int idx = i;

                Debug.Log($"start : {coroutines[idx].Method.Name}");
                StartCoroutine(coroutines[i](() =>
                {
                    finished[idx] = true;
                    Debug.Log($"finish : {coroutines[idx].Method.Name}");
                }));
            }
            yield return new WaitUntil(() => finished.All(x => x == true));
            _finished = true;
        }

        private IEnumerator StartTest(Action onComplete)
        {
            bool finished = false;
            var coroutineController = new CoroutineController(this);
            var stateMachine = new CoroutineStateMachine<State>(coroutineController);
            stateMachine.AddState(State.A, A);
            stateMachine.AddState(State.B, B);
            stateMachine.AddState(State.C, C);
            stateMachine.Start(State.A);

            yield return new WaitForSeconds(5.0f);
            Assert.That(finished);
            onComplete();
            IEnumerator A()
            {
                yield return coroutineController.WaitForSeconds(1);
                yield return stateMachine.ChangeState(State.B);
                Assert.That(false);
            }
            IEnumerator B()
            {
                yield return coroutineController.WaitForSeconds(1);
                yield return stateMachine.ChangeState(State.C);
                Assert.That(false);
            }
            IEnumerator C()
            {
                yield return coroutineController.WaitForSeconds(1);
                finished = true;
            }
        }
        private IEnumerator StackOverFlowTest(Action onComplete)
        {
            var coroutineController = new CoroutineController(this);
            var stateMachine = new CoroutineStateMachine<State>(coroutineController);
            stateMachine.AddState(State.A, A);
            stateMachine.AddState(State.B, B);
            stateMachine.Start(State.A);
            yield return new WaitForSeconds(5.0f);
            onComplete();
            IEnumerator A()
            {
                yield return coroutineController.WaitForFrame();
                yield return stateMachine.ChangeState(State.B);
                Assert.That(false);
            }
            IEnumerator B()
            {
                yield return coroutineController.WaitForFrame();
                yield return stateMachine.ChangeState(State.A);
                Assert.That(false);
            }
        }
        private IEnumerator KillTest(Action onComplete)
        {
            var coroutineController = new CoroutineController(this);
            var stateMachine = new CoroutineStateMachine<State>(coroutineController);
            stateMachine.AddState(State.A, A);
            stateMachine.AddState(State.B, B);
            stateMachine.Start(State.A);

            yield return new WaitForSeconds(1.0f);
            stateMachine.Kill();
            onComplete();
            IEnumerator A()
            {
                yield return coroutineController.WaitForSeconds(1.0f);
                yield return stateMachine.ChangeState(State.B);
                Assert.That(false);
            }
            IEnumerator B()
            {
                yield return coroutineController.WaitForSeconds(1.0f);
                Assert.That(false);
            }
        }
    }
}
