using System.Collections.Generic;
using UnityEngine;

namespace Eduzo.Games.BoxingMayhem.Sound {
    public class BoxingMayhemSoundManager : MonoBehaviour {
        public enum SoundId {
            ButtonClick, CorrectAnswer, IncorrectAnswer, BGMusic
        }
        public static BoxingMayhemSoundManager Instance { get; private set; }

        [Header("Sources")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        [Header("Data")]
        [SerializeField] private Data.SoundDataSO _soundDataSO;

        private Dictionary<string, AudioClip> _soundDict;
        #region Mono
        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _soundDict = new Dictionary<string, AudioClip>();
            foreach (var sound in _soundDataSO.Sounds) {
                _soundDict[sound.Name] = sound.AudioClip;
            }
        }
        #endregion

        #region Event Listeners

        #endregion
        public void PlaySFX(SoundId id) {
            string soundId = id.ToString();
            if (_soundDict.TryGetValue(soundId, out AudioClip clip)) {
                _sfxSource.PlayOneShot(clip);
            }
        }

        public void PlayMusic(SoundId id) {
            string soundId = id.ToString();
            if (_soundDict.TryGetValue(soundId, out AudioClip clip)) {
                _musicSource.clip = clip;
                _musicSource.loop = true;
                _musicSource.Play();
            }
        }
        public void StopMusic() {
            _musicSource.Stop();
        }
    }
}