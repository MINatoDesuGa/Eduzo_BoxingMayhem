using UnityEngine;

namespace Eduzo.Games.BoxingMayhem.Sound.Data {
    [CreateAssetMenu(fileName = "BoxingMayhemSoundDataSO", menuName = "Eduzo/Games/BoxingMayhem/SoundData")]
    public class SoundDataSO : ScriptableObject {
        public SoundData[] Sounds;
    }
    [System.Serializable]
    public class SoundData {
        public string Name;
        public AudioClip AudioClip;
    }
}