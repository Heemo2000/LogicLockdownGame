using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using Game.Core;

namespace Game.SoundManagement
{
    public class SoundManager : GenericSingleton<SoundManager>
    {
        

        [SerializeField]private Sound[] sounds;
        [SerializeField]private GameSound startingMusic;
        [SerializeField]private AudioMixerGroup musicGroup;
        [SerializeField]private AudioMixerGroup sfxGroup;
        private Dictionary<GameSound,KeyValuePair<Sound, AudioSource>> soundDictionary;

        public Sound GetSoundInfo(GameSound soundType)
        {
            if(soundDictionary.TryGetValue(soundType, out var soundPair))
            {
                return soundPair.Key;
            }

            return null;
        }
        public void PlaySound(GameSound soundType)
        {
            Play(soundType);
        }

        public void PlaySoundInstantly(GameSound soundType)
        {
            Play(soundType, true);
        }

        public void PauseAllSound()
        {
            foreach(var pair in soundDictionary.Values)
            {
                pair.Value.Pause();
            }
        }

        public void ResumeAllSound()
        {
            foreach(var pair in soundDictionary.Values)
            {
                pair.Value.Play();
            }
        }

        public void StopAllSound()
        {
            foreach(var pair in soundDictionary.Values)
            {
                pair.Value.Stop();
            }
        }

        public void SetMusicVolume(float volume)
        {
            musicGroup.audioMixer.SetFloat("musicVol", volume);
        }

        public void SetSFXVolume(float volume)
        {
            sfxGroup.audioMixer.SetFloat("sfxVol", volume);
        }

        public void SetPitch(GameSound soundType ,float pitch)
        {
            if(soundDictionary.TryGetValue(soundType,out var soundPair))
            {
                soundPair.Value.pitch = pitch;
            }
            else
            {
                Debug.LogError("Sound to modify pitch not found: " + soundType.ToString());
            }
        }

        protected override void Awake() {
            
            base.Awake();
            //DontDestroyOnLoad(gameObject);
            soundDictionary = new Dictionary<GameSound, KeyValuePair<Sound, AudioSource>>();        
            foreach(Sound sound in sounds)
            {
                GameObject audioSourceObj = new GameObject(sound.gameSound.ToString());
                audioSourceObj.transform.parent = transform;
                AudioSource audioSourceComp = audioSourceObj.AddComponent<AudioSource>();

                switch(sound.category)
                {
                    case SoundCategory.Music:
                                                audioSourceComp.outputAudioMixerGroup = musicGroup;
                                                break;
                    case SoundCategory.SFX:     audioSourceComp.outputAudioMixerGroup = sfxGroup;
                                                break;
                }
                soundDictionary.Add(sound.gameSound,new KeyValuePair<Sound, AudioSource>(sound, audioSourceComp));
            }
        }

        private void Start() 
        {
            Play(startingMusic);
        }
        private void Play(GameSound soundType, bool oneShot = false)
        {
            if(soundDictionary.TryGetValue(soundType,out var soundPair))
            {
                soundPair.Value.clip = soundPair.Key.clips[UnityEngine.Random.Range(0, soundPair.Key.clips.Length)];
                soundPair.Value.pitch = soundPair.Key.pitch;
                soundPair.Value.loop = soundPair.Key.loop;
                soundPair.Value.volume = soundPair.Key.volume;
                if(oneShot)
                {
                    soundPair.Value.PlayOneShot(soundPair.Value.clip);
                }
                else
                {
                    soundPair.Value.Play();
                }
            }
            else
            {
                Debug.LogError("Sound to play not found: " + soundType.ToString());
            }        
        }
    }

}
