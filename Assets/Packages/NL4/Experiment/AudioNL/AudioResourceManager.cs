using System;
using System.Collections.Generic;
using UnityEngine;
namespace AudioNL
{
    public class AudioResourceManager : MonoBehaviour
    {
        [SerializeField]
        private AudioPlayData[] _audioPlayDatas;
        public Dictionary<string, BGMDataProvider> datas { get; private set; } = new Dictionary<string, BGMDataProvider>();
        private void Awake()
        {
            foreach (var data in _audioPlayDatas)
            {
                BGMDataProvider provider;
                if (data.LoadStrategy == AudioLoadStrategy.LoadOnPlay)
                {
                    provider = new BGMDataProvider(data.GetIntroClip(), data.GetLoopClip(), data.LoadStrategy);
                }
                else
                {
                    provider = new BGMDataProvider(data.GetIntroClip, data.GetLoopClip, data.LoadStrategy);
                }
                datas.Add(data.GetKey(), provider);
            }
        }

        public BGMDataProvider GetBGMDataProvider(AudioPlayData data)
        {
            if (datas.TryGetValue(data.GetKey(), out BGMDataProvider provider))
            {
                return provider;
            }
            else
            {
                Debug.LogError("BGMDataProvider not found for the provided AudioPlayData.");
                return null;
            }
        }
    }


}