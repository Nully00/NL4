using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
namespace AudioNL
{
    [CreateAssetMenu(fileName = "Play Data", menuName = "AudioNL/AudioPlayData")]
    public class AudioPlayData : ScriptableObject
    {
        [SerializeField]
        private string name1 = "Name1";
        [SerializeField]
        private string name2 = "Name2";
        [SerializeField]
        private string _introClipPath;
        [SerializeField]
        private string _loopClipPath;
        [SerializeField]
        private float overlapTime;
        [SerializeField]
        private AudioLoadStrategy _loadStrategy;
        public string Name1 => name1;
        public string Name2 => name2;
        public float OverlapTime => overlapTime;
        public AudioLoadStrategy LoadStrategy => _loadStrategy;
        public AudioClip GetIntroClip()
        {
            AudioClip clip = Resources.Load<AudioClip>(_introClipPath);
            return clip;
        }

        public AudioClip GetLoopClip()
        {
            AudioClip clip = Resources.Load<AudioClip>(_loopClipPath);
            if (clip == null)
            {
                Debug.LogError("Loop clip not found at path: " + _loopClipPath);
            }
            return clip;
        }

        internal string GetKey()
        {
            return $"{_introClipPath}{_loopClipPath}{name1}{name2}";
        }
#if UNITY_EDITOR
        private void OnValidate()
        {

            if (_loadStrategy == AudioLoadStrategy.LoadAtStart)
            {
                SetPreload(true);
            }
            else
            {
                SetPreload(false);
            }
        }
        private void SetPreload(bool preload)
        {
            SetPreload(_loopClipPath, preload);
            SetPreload(_introClipPath, preload);
        }
        private void SetPreload(string assetPathWithoutExtension, bool preload)
        {
            string[] supportedExtensions = { ".wav", ".mp3", ".ogg", ".aif" };
            foreach (string extension in supportedExtensions)
            {
                string fullPath = Path.Combine("Assets/Resources", assetPathWithoutExtension + extension);
                var audioImporter = AssetImporter.GetAtPath(fullPath) as AudioImporter;
                if (audioImporter != null)
                {
                    if (audioImporter.preloadAudioData != preload)
                    {
                        audioImporter.preloadAudioData = preload;
                        audioImporter.SaveAndReimport();
                    }
                    break;
                }
            }
        }
#endif
    }
}