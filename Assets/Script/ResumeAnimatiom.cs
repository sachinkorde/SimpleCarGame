using System;
using UnityEngine;


    public class ResumeAnimatiom : MonoBehaviour
    {
        public Action OnAnimComplete;

        public void PauseAnimCompleted()
        {
            gameObject.SetActive(false);
            GameManager.instance.StartDelay();
        }

        public void PlaySound()
        {
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[19]);
            //SoundManager.instance.PlayOneShot(gameAudioClip.resume_countdown);
        }
    }

