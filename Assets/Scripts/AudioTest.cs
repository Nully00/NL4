using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioNL;

public class AudioTest : MonoBehaviour
{
    [SerializeField]
    private AudioPlayData _audioPlayData;
    [SerializeField]
    private AudioPlayData _audioPlayData2;
    [SerializeField]
    private BGMPlayer _bgmPlayer;
    private async UniTask Start()
    {
        _bgmPlayer.Play(_audioPlayData);

        await UniTask.WaitForSeconds(2.0f);
        //_bgmPlayer.Stop();

        _bgmPlayer.Pause();
        await UniTask.WaitForSeconds(6.0f);
        _bgmPlayer.Restart();
        //SceneManager.LoadScene("B");
    }
}
