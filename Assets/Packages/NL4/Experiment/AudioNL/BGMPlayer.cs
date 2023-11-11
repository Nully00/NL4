using Cysharp.Threading.Tasks;
using NLTask;
using System;
using UnityEngine;
namespace AudioNL
{
    public class BGMPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioResourceManager _resourceManager;
        [SerializeField]
        private AudioSourcePool _audioSourcePool;
        private BGMDataProvider _currentProvider;
        private PausableToken _currentPausedToken;
        private bool _paused = false;
        public void Play(AudioPlayData audioPlayData)
        {
            if (audioPlayData == null)
            {
                throw new ArgumentNullException(nameof(audioPlayData), "Provided AudioPlayData is null.");
            }
            _paused = false;
            Stop();
            _currentPausedToken = new PausableToken(this, () => _paused);
            _ = PlayInternal(audioPlayData, _currentPausedToken);
        }
        private async UniTask PlayInternal(AudioPlayData audioPlayData, PausableToken token)
        {
            _currentProvider = _resourceManager.GetBGMDataProvider(audioPlayData);
            AudioClip loop = _currentProvider.GetLoop();
            AudioClip intro = _currentProvider.GetIntro();

            if (intro != null)
            {
                PlayClip(intro);
            }
            float offset = 0;
            do
            {
                var currentSource = PlayClip(loop, offset);
                await PausableTask.WaitUntil(() => currentSource.time >= audioPlayData.OverlapTime, token);
                offset = currentSource.time - audioPlayData.OverlapTime;
            }
            while (true);
        }

        private AudioSource PlayClip(AudioClip clip,float offset = 0)
        {
            AudioSource newSource = _audioSourcePool.GetAvailableAudioSource();
            newSource.clip = clip;
            newSource.Play();
            newSource.time = offset;
            return newSource;
        }
        public void Stop()
        {
            if (_currentProvider != null)
            {
                _audioSourcePool.StopAllAudioSources();
                _audioSourcePool.ReleaseAllAudioClip();
                _currentProvider.Release();
                _currentProvider = null;
                _currentPausedToken.Cancel();
            }
        }
        public void Pause()
        {
            _currentPausedToken.Pause();
            _audioSourcePool.PauseAllAudioSources();
        }

        public void Restart()
        {
            _currentPausedToken.Resume();
            _audioSourcePool.RestartAllAudioSources();
        }
    }
}