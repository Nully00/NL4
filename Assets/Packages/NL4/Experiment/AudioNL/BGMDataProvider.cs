using Cysharp.Threading.Tasks;
using NLTask;
using System;
using UnityEngine;
namespace AudioNL
{
    public class BGMDataProvider
    {
        private AudioLoadStrategy _loadStrategy;
        private Func<AudioClip> _getIntro;
        private Func<AudioClip> _getLoop;
        private AudioClip _intro = null;
        private AudioClip _loop = null;
        public BGMDataProvider(AudioClip intro, AudioClip loop, AudioLoadStrategy audioLoadStrategy)
        {
            if (intro == null || loop == null)
            {
                throw new ArgumentNullException("Intro and/or loop clips cannot be null.");
            }
            this._intro = intro;
            this._loop = loop;
            this._loadStrategy = audioLoadStrategy;
        }

        public BGMDataProvider(Func<AudioClip> getIntro, Func<AudioClip> getLoop, AudioLoadStrategy audioLoadStrategy)
        {
            if (getIntro == null || getLoop == null)
            {
                throw new ArgumentNullException("Intro and/or loop clip providers cannot be null.");
            }
            this._getIntro = getIntro;
            this._getLoop = getLoop;
            this._loadStrategy = audioLoadStrategy;
        }

        public AudioClip GetIntro()
        {
            if (_intro == null)
            {
                _intro = _getIntro();
            }
            return _intro;
        }
        public AudioClip GetLoop()
        {
            if (_loop == null)
            {
                _loop = _getLoop();
            }
            return _loop;
        }

        public void Release()
        {
            if (_loadStrategy == AudioLoadStrategy.LoadForEachPlay)
            {
                _intro.UnloadAudioData();
                _loop.UnloadAudioData();
                _intro = null;
                _loop = null;
                _ = PausableTask.WaitTaskThenInvoke(UniTask.Yield().ToUniTask(), () =>
                {
                    Resources.UnloadAsset(_loop);
                    Resources.UnloadAsset(_intro);
                });
            }
        }
    }
}