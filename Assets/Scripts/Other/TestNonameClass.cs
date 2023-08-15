using System.Collections;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using System.Buffers;

public class TestNonameClass : MonoBehaviour
{
    private CoroutineController _coroutineController;
    [SerializeField]
    private float _floatValue = 0;
    [SerializeField]
    private bool _flagA = false;
    [SerializeField]
    private bool _flagB = false;
    void Start()
    {
        _coroutineController = new CoroutineController(this);
    }

    [Button]
    void StartCotoutine_Normal()
    {
        _coroutineController.Run(TestA());

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
    }


    

    [Button]
    void StartCotoutine_RunParallel()
    {
        _coroutineController.Run(TestA());

        IEnumerator TestA()
        {
            Debug.Log("Start TestA");
            yield return _coroutineController.WaitRunChild(TestB(), TestC());
            Debug.Log("End TestA");
        }

        IEnumerator TestB()
        {
            Debug.Log("Start TestB");
            yield return _coroutineController.WaitForSeconds(2);
            Debug.Log("End TestB");
        }

        IEnumerator TestC()
        {
            Debug.Log("Start TestC");
            yield return _coroutineController.WaitForSeconds(3);
            Debug.Log("End TestC");
        }
    }
    [Button]
    void StartCotoutine_RunWhenTrue()
    {
        _coroutineController.Run(TestA());

        IEnumerator TestA()
        {
            Debug.Log("Start TestA");
            yield return _coroutineController.WaitRunWhenTrue(
                (()=> true,TestB()),
                (()=> _flagA,TestC()));
            Debug.Log("End TestA");
        }

        IEnumerator TestB()
        {
            Debug.Log("Start TestB");
            yield return _coroutineController.WaitForSeconds(2);
            yield return _coroutineController.KillFromInside();
            Debug.Log("End TestB");
        }

        IEnumerator TestC()
        {
            Debug.Log("Start TestC");
            yield return _coroutineController.WaitForSeconds(3);
            Debug.Log("End TestC");
        }
        
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
