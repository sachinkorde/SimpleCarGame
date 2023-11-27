using UnityEngine;
using System.Collections.Generic;


    public enum HomeScene_gameAudioClips
    {
        swipeRight,
        swiprLeft,
        smoothPlayerSwipe,
        OnClickSticketLoadBtn
    }

    [System.Serializable]
    public class GameAudioClipInfo
    {
        public HomeScene_gameAudioClips clip;
        public AudioClip audioClip;
    }

    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        public AudioSource aud;
        public AudioSource homeScene_aud;
        public AudioSource gameBgSound;
        public AudioSource localize_unlock_characters;
        public AudioSource resultPlayer_conffetti_AudioSource;
        public AudioSource resultPlayer_yeah_AudioSource;

        public AudioClip ckGO_BGM;
        public AudioClip[] inGameClips;

        public List<GameAudioClipInfo> gameAudios = new();

        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public static void DestroyInstance()
        {
            Destroy(instance.gameObject);
        }

        public AudioClip GetAudioClip(HomeScene_gameAudioClips clip)
        {
            for (int i = 0; i < gameAudios.Count; i++)
            {
                if (gameAudios[i].clip == clip) return gameAudios[i].audioClip;
            }

            return null;
        }

        public void PlayOneShotClip(HomeScene_gameAudioClips clip, bool loop = false)
        {
            homeScene_aud.Stop();
            AudioClip audioClip = GetAudioClip(clip);
            homeScene_aud.clip = audioClip;
            homeScene_aud.loop = loop;
            homeScene_aud.PlayOneShot(audioClip);
        }
    }