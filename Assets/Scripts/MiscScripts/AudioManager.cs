using System;
using Character;
using SaveSystem;
using UnityEngine;

namespace MiscScripts
{
    public class AudioManager : MonoBehaviour, IDataPersistence
    {
        public static AudioManager Instance { get; private set; }
    
        [Header("Music")]
        [SerializeField] AudioSource musicSource;
        public AudioClip defaultMusic;
    
        [Header("Sound Effects")]
        [SerializeField] AudioSource effectsSource;

        [Header("Ambiance")]
        [SerializeField] private AudioSource ambianceSource;
    
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SaveData data = DataPersistenceManager.Instance.GetData();
            if (data != null)
            {
                PlayerCharacter.Instance.GetPlayerUserInterface().settingsScript.OnMusicSliderUpdate(data.musicVolume);
                PlayerCharacter.Instance.GetPlayerUserInterface().settingsScript.OnEffectSliderUpdate(data.effectVolume);
                PlayerCharacter.Instance.GetPlayerUserInterface().settingsScript.OnAmbianceSliderUpdate(data.ambianceVolume);
            }
        }

        public void PlaySoundEffect(AudioClip clip)
        {
            effectsSource.PlayOneShot(clip);
        }

        public void PlayMusicEffect(AudioClip clip, bool musicLoop)
        {
            musicSource.clip = clip;
            musicSource.Play();
            musicSource.loop = musicLoop;
        }

        public void PlayAmbiance(AudioClip clip, bool ambianceLoop)
        {
            ambianceSource.clip = clip;
            ambianceSource.Play();
            ambianceSource.loop = ambianceLoop;
        }

        /// <summary>
        /// Stop all sound in the game.
        /// </summary>
        public void Stop()
        {
            musicSource.Stop();
            effectsSource.Stop();
            ambianceSource.Stop();
        }

        public void StopSoundEffect()
        {
            effectsSource.Stop();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void StopAmbiance()
        {
            ambianceSource.Stop();
        }

        public AudioSource GetMusicSource()
        {
            return musicSource;
        }

        public AudioSource GetEffectSource()
        {
            return effectsSource;
        }

        public AudioSource GetAmbianceSource()
        {
            return ambianceSource;
        }

        public void LoadData(SaveData data)
        {
            return;
        }

        public void SaveData(ref SaveData data)
        {
            data.musicVolume = musicSource.volume;
            data.effectVolume = effectsSource.volume;
            data.ambianceVolume = ambianceSource.volume;
        }

        public void PlayDefaultMusic()
        {
            musicSource.clip = defaultMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}
