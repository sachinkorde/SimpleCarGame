using UnityEngine;


    public class OnEndChestAnimation : MonoBehaviour
    {
        public void SoundPlay()
        {
            GameManager.instance.meterAudio.loop = true;
            GameManager.instance.meterAudio.Play();
        }

        public void SoundOFF()
        {
            GameManager.instance.OnEndPowerUpTitlesAnim();
        }

        public void OffCoin()
        {
            ScoreManager.instance.coinCollectAnimation.gameObject.SetActive(false);
        }

        public void StartTitleAnim()
        {
            GameManager.instance.OnStartPowerUpTitlesAnim();
        }

        public void CoinMeterMainSetToIdle()
        {
            GameManager.instance.coinMeterMain.SetTrigger("idle");
            GameManager.instance.OnEndAllCoinMeterAnim();
        }

        public void StickerGameOrientation()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }

        public void BottomAnimation()
        {
            StickerGame.instance.BottomAnimAtStart();
        }

        public void PlayReplayButtonSound()
        {
            GetComponent<AudioSource>().Play();
        }

        public void ChangeReplayButtonAnimation()
        {
            GetComponent<Animator>().SetTrigger("replayBtnAnm");
            ScoreManager.instance.PlayerStandAnimation();
        }
    
}
