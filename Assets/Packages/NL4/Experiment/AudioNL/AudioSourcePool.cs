using Cysharp.Threading.Tasks;
using NLTask;
using System.Collections.Generic;
using UnityEngine;
namespace AudioNL
{
    public class AudioSourcePool : MonoBehaviour
    {
        private List<AudioSource> pool;

        void Awake()
        {
            Resources.UnloadUnusedAssets();
            pool = new List<AudioSource>();
        }

        public AudioSource GetAvailableAudioSource()
        {
            foreach (var source in pool)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }

            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            newSource.loop = false;
            pool.Add(newSource);
            return newSource;
        }
        public void StopAllAudioSources()
        {
            foreach (var source in pool)
            {
                source.Stop();
            }
        }
        public void PauseAllAudioSources()
        {
            foreach (var source in pool)
            {
                source.Pause();
            }
        }
        public void SetVolume(float volume)
        {
            foreach (var source in pool)
            {
                source.volume = volume;
            }
        }
        public void ReleaseAllAudioClip()
        {
            foreach (var source in pool)
            {
                source.clip = null;
            }
        }
        public void RestartAllAudioSources()
        {
            foreach (var source in pool)
            {
                source.Play();
            }
        }

        private void OnDestroy()
        {
            ReleaseAllAudioClip();
            _ = PausableTask.WaitTaskThenInvoke(UniTask.Yield().ToUniTask(), () =>
            {
                Resources.UnloadUnusedAssets();
            });
        }
    }
}