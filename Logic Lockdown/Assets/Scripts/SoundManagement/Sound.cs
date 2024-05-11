using UnityEngine;
using UnityEngine.Audio;


namespace Game.SoundManagement
{
    public enum SoundCategory
    {
        Music,
        SFX
    }

    [CreateAssetMenu(menuName = "Scriptable Objects/Sound Data",fileName = "Sound Data")]
    public class Sound : ScriptableObject
    {
        public SoundCategory category;
        public GameSound gameSound;
        public AudioClip[] clips;
        
        [Range(-3f, 3f)]
        public float pitch = 1;

        [Range(0f,1f)]
        public float volume = 0.5f;
        public bool loop = false;
    }

}

