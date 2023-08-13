using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestNonameClass : MonoBehaviour
{
    private CoroutineController _coroutineController;
    void Start()
    {
        _coroutineController = new CoroutineController(this);
    }

    [Button]
    void StartCotoutine()
    {
        _coroutineController.Run(TestA());
    }


    IEnumerator TestA()
    {
        Debug.Log("Start TestA");
        yield return _coroutineController.WaitForSeconds(2);
        yield return _coroutineController.WaitRunChild(TestB());
        Debug.Log("End TestA");
    }

    IEnumerator TestB()
    {
        Debug.Log("Start TestB");
        yield return _coroutineController.WaitForSeconds(2);
        yield return _coroutineController.WaitRunChild(TestC());
        Debug.Log("End TestB");
    }

    IEnumerator TestC()
    {
        Debug.Log("Start TestC");
        yield return _coroutineController.WaitForSeconds(2);
        Debug.Log("End TestC");
    }
    [Button]
    private void Kill()
    {
        _coroutineController.Kill();
    }
    [Button]
    private void Pause()
    {
        _coroutineController.Pause();
    }
    [Button]
    private void Restart()
    {
        _coroutineController.Restart();
    }
}
