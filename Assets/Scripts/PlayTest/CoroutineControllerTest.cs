using System;
using System.Buffers;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

public class CoroutineControllerTest 
{
    [UnityTest]
    public IEnumerator CoroutineTest()
    {
        yield return new MonoBehaviourTest<CoroutineControllerMonoBehaviourTest>();
    }
    public class CoroutineControllerMonoBehaviourTest : MonoBehaviour, IMonoBehaviourTest
    {
        private bool _finished = false;
        public bool IsTestFinished => _finished;

        private float _time;

        private IEnumerator Start()
        {
            Func<Action, IEnumerator>[] coroutines =
            {
                RunTest,PauseTest,KillTest,KillMultipleTest,KillNoWaitTest,
                DelayActionTest,DelayActionKillTest,RunWhenTrueTest1,RunWhenTrueTest2,OverTimeActionTest,
                ThorwAndFinallyTest,CompleteTest
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
        private void Update()
        {
            _time += Time.deltaTime;
        }
        private IEnumerator RunTest(Action onComplete)
        {
            var coroutineController = new CoroutineController(this);
            yield return coroutineController.WaitRun(TestA());
            onComplete();
            IEnumerator TestA()
            {
                yield return coroutineController.WaitForSeconds(2);
            }
        }
        private IEnumerator PauseTest(Action onComplete)
        {
            float waitForSeconds = 2.0f;
            float pauseTime = 5.0f;

            var coroutineController = new CoroutineController(this);
            float startTime = _time;
            yield return coroutineController.WaitRun(TestA());
            float endTime = _time;

            var comparer = FloatEqualityComparer.Instance;
            Assert.That(Mathf.Abs((waitForSeconds + pauseTime) - (endTime - startTime)), Is.LessThan(0.25f));
            onComplete();
            IEnumerator TestA()
            {
                yield return PauseTime(pauseTime);
                yield return coroutineController.WaitForSeconds(waitForSeconds);
            }

            IEnumerator PauseTime(float time)
            {
                coroutineController.Pause();
                yield return new WaitForSeconds(time);
                coroutineController.Restart();
            }
        }
        private IEnumerator KillTest(Action onComplete)
        {
            var coroutineController = new CoroutineController(this);
            onComplete();
            yield return coroutineController.WaitRun(TestA());
            Assert.That(false);
            IEnumerator TestA()
            {
                yield return coroutineController.WaitForFrame();
                yield return coroutineController.KillFromInside();
                yield return coroutineController.WaitForSeconds(50);
            }
        }
        private IEnumerator KillMultipleTest(Action onComplete)
        {
            var coroutineController = new CoroutineController(this);
            onComplete();
            yield return coroutineController.WaitRun(TestA());
            Assert.That(false);
            IEnumerator TestA()
            {
                yield return coroutineController.WaitRunChild(TestB());
                Assert.That(false);

            }
            IEnumerator TestB()
            {
                yield return coroutineController.WaitRunChild(TestC());
                Assert.That(false);

            }
            IEnumerator TestC()
            {
                yield return coroutineController.KillFromInside();
                Assert.That(false);

            }
        }
        private IEnumerator KillNoWaitTest(Action onComplete)
        {
            var coroutineController = new CoroutineController(this);
            onComplete();
            yield return coroutineController.WaitRun(TestA());
            Assert.That(false);
            IEnumerator TestA()
            {
                yield return coroutineController.WaitRunChild(TestB(), TestC());
                Assert.That(false);

            }
            IEnumerator TestB()
            {
                yield return coroutineController.WaitForSeconds(10);
                Assert.That(false);

            }
            IEnumerator TestC()
            {
                yield return coroutineController.KillFromInside();
                Assert.That(false);

            }
        }
        private IEnumerator DelayActionTest(Action onComplete)
        {
            var coroutineController = new CoroutineController(this);
            bool finished = false;
            coroutineController.DelayAction(3, () => finished = true);
            yield return coroutineController.WaitUntil(() => finished);
            Assert.That(true);
            onComplete();
        }
        private IEnumerator DelayActionKillTest(Action onComplete)
        {
            var coroutineController = new CoroutineController(this);
            bool finished = false;
            coroutineController.DelayAction(3.0f, () => finished = true);
            yield return coroutineController.WaitForSeconds(2.0f);
            coroutineController.Kill();
            yield return coroutineController.WaitForSeconds(2.0f);
            Assert.That(finished == false);
            onComplete();
        }
        private IEnumerator RunWhenTrueTest1(Action onComplete)
        {
            float waitTime = 1.0f;
            bool flagA = false;
            var coroutineController = new CoroutineController(this);
            float startTime = _time;
            yield return coroutineController.WaitRunWhenTrue(
                    (() => true, TestA()),
                    (() => flagA, TestB()));
            float endTime = _time;

            Assert.That(Mathf.Abs((waitTime * 2) - (endTime - startTime)), Is.LessThan(0.1f));
            onComplete();
            IEnumerator TestA()
            {
                yield return coroutineController.WaitForSeconds(waitTime);
                flagA = true;
            }

            IEnumerator TestB()
            {
                yield return coroutineController.WaitForSeconds(waitTime);
            }
        }
        private IEnumerator RunWhenTrueTest2(Action onComplete)
        {
            float waitTime = 1.0f;
            bool flagA = false;
            bool flagB = false;
            var coroutineController = new CoroutineController(this);
            coroutineController.DelayAction(2, () => flagB = true);
            yield return coroutineController.WaitRunWhenTrue(
                    (() => true, TestA()),
                    (() => flagA, TestB()),
                    (() => flagB, TestB()));

            Assert.That(true);
            onComplete();
            IEnumerator TestA()
            {
                yield return coroutineController.WaitForSeconds(waitTime);
                flagA = true;
            }

            IEnumerator TestB()
            {
                yield return coroutineController.WaitForSeconds(waitTime);
            }
        }
        private IEnumerator OverTimeActionTest(Action onComplete)
        {
            var coroutineController = new CoroutineController(this);
            var result = new System.Collections.Generic.List<float>();
            float startTime = _time;
            yield return coroutineController.WaitOverTimeAction(1.5f, 
                x => Assert.That(Mathf.Abs(Mathf.Min(_time - startTime,1.5f) - x),Is.LessThan(0.05f)));
            //Debug.Log($"WaitOverTimeAction : {string.Join(',', result.Select(x => x.ToString()).ToArray())}");
            result.Clear();
            startTime = _time;
            yield return coroutineController.WaitOverTimeAction01(1.5f, 
                x => Assert.That(Mathf.Abs(Mathf.Min(((_time - startTime) / 1.5f), 1.0f) - x), Is.LessThan(0.05f)));
            //Debug.Log($"WaitOverTimeAction01 : {string.Join(',', result.Select(x => x.ToString()).ToArray())}");
            onComplete();
        }
        private IEnumerator ThorwAndFinallyTest(Action onComplete)
        {
            var coroutineController = new CoroutineController(this);
            int[] array = ArrayPool<int>.Shared.Rent(10);
            bool finallied = false;
            bool onErrored = false;
            try
            {
                yield return coroutineController.WaitRunChild(Error(array), _ => onErrored = true);
            }
            finally
            {
                ArrayPool<int>.Shared.Return(array);
                finallied = true;
            }

            Assert.That(finallied);
            Assert.That(onErrored);
            onComplete();
            IEnumerator Error(int[] array)
            {
                yield return new WaitForSeconds(1.0f);
                throw new System.Exception();
            }
        }
        private IEnumerator CompleteTest(Action onComplete)
        {
            var coroutineController = new CoroutineController(this);
            bool finallied = false;
            coroutineController.RunChild(A(), () => finallied = true);
            yield return coroutineController.WaitForSeconds(1.5f);
            Assert.That(finallied);
            onComplete();
            IEnumerator A()
            {
                finallied = false;
                yield return coroutineController.WaitForSeconds(1.0f);
            }
        }
    }
}

