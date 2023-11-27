using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;

        int unlockPlayerCounter = 0;
        int unlockStickerCounter = 0;
        int magicCounter = 0;
        int magicDecision = 0;
        int firstTimeOpen = 0;
        int whichCoinsRelaease;
        public int tempScore;
        public int coinMeterValue = 0;
        int getNewCoins = 0;
        public int coinsCount, UnlockRandomNumber;
        public int prizelogicounter = 10;

        public TMP_Text saveScore, gameOver_score, gameOver_coins, finalCoinScore, coinMeterText, coinsAfterBox, coinValues;

        public GameObject unlockStickers, gameOverTrophy, gameOverTrophyBg, prizebox, prizeboxBg, openPrizebox, closePrizebox;
        public GameObject resultPlayer, gameOverPanel, resultbg, prizebgResult, highScoreImage, coinsValueForPrizeBox; //magicBoxResultBg

        public UnityEngine.UI.Button pauseButton;
        public UnityEngine.UI.Button backHomeButton;
        public UnityEngine.UI.Button boxOpen;
        public UnityEngine.UI.Button tapButton;
        public UnityEngine.UI.Button replayBtn;

        public SkeletonGraphic _skeletonAnimResult;
        public SkeletonGraphic _mystryBoxSpine;
        
        public int[] coinValuePrize = { 5, 10, 15, 20 };
        public List<GameObject> stickersForUnlock;
        public List<GameObject> playersForUnlock;

        public Animator coinCollectAnimation;
        public Animator replayButton;
        
        public ParticleSystem startGlow;
        public ParticleSystem highScoreConfetti;
        public ParticleSystem coinSpark;
        public ParticleSystem playerLandedParticleRight;
        public ParticleSystem playerLandedParticleLeft;

        float tempScoreAnim = 0;
        float tempCoinAnim = 0;
        float tempwaitTime = 0;
        float waitTimeOfClip = 0;
        
        private void Start()
        {
            instance = this;
            _skeletonAnimResult = resultPlayer.GetComponent<SkeletonGraphic>();
            _mystryBoxSpine = _mystryBoxSpine.GetComponent<SkeletonGraphic>();
            if (!string.IsNullOrEmpty(PlayerPrefsManager.SelectedPlayerName))
            {
                _skeletonAnimResult.Skeleton.SetSkin(PlayerPrefsManager.SelectedPlayerName);
            }
            else
            {
                PlayerPrefsManager.SelectedPlayerName = "capkid";
                _skeletonAnimResult.Skeleton.SetSkin(PlayerPrefsManager.SelectedPlayerName);
            }
            
            StartAgain();
        }

        public void StartAgain()
        {
            gameOver_score.text = "000000";
            gameOver_coins.text = "0000";
            coinsAfterBox.text = "00";
            finalCoinScore.text = "";
            coinCollectAnimation.gameObject.SetActive(false);
            coinSpark.Stop();
            PlayerPrefsManager.CurrentScore = 0;
            PlayerPrefsManager.Coins = 0;
            backHomeButton.interactable = false;
            tempScore = 0;
            coinsCount = 0;
            coinMeterValue = PlayerPrefs.GetInt("coinMeterValue");
            coinMeterText.text = PlayerPrefs.GetInt("coinMeterValue").ToString();
            getNewCoins = 0;
            pauseButton.interactable = true;
            coinsAfterBox.gameObject.SetActive(false);
            gameOverPanel.transform.localScale = Vector3.zero;
            gameOverTrophy.SetActive(false);
            closePrizebox.SetActive(true);
            openPrizebox.SetActive(false);
            highScoreConfetti.gameObject.SetActive(true);
            saveScore.text = "";
            _mystryBoxSpine.AnimationState.SetAnimation(0, "wobble", true);
            replayButton.gameObject.SetActive(false);
            replayBtn.enabled = false;
            SoundManager.instance.aud.Stop();

            for (int i = 0; i < stickersForUnlock.Count; i++)
            {
                stickersForUnlock[i].SetActive(false);
            }

            for (int i = 0; i < playersForUnlock.Count; i++)
            {
                playersForUnlock[i].SetActive(false);
            }

            coinsValueForPrizeBox.SetActive(false);
        }

        public void GameOver()
        {
            _skeletonAnimResult.AnimationState.SetAnimation(0, "end_game", false);
            StartCoroutine(GameOverScreen());
        }

        public IEnumerator GameOverScreen()
        {
            pauseButton.interactable = false;
            yield return new WaitForSeconds(1.2f);
            SoundManager.instance.aud.Stop();
            gameOverPanel.transform.localScale = Vector3.one;

            ScoreAnimation();

            resultPlayer.SetActive(true);
            _skeletonAnimResult.Skeleton.SetSkin(PlayerPrefsManager.SelectedPlayerName);
            _skeletonAnimResult.AnimationState.SetAnimation(0, "end_game2", true);
            
            finalCoinScore.text = coinMeterValue.ToString();
            PlayerDiedAnimations.instance.PreRestartGame();
        }

        public void ScoreAnimation()
        {
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[14], 1);
            waitTimeOfClip = SoundManager.instance.inGameClips[14].length;

            InvokeRepeating(nameof(UpdateScore), 0f, waitTimeOfClip / tempScore);
            InvokeRepeating(nameof(UpdateClipTime), 0f, 1.0f);

            if(coinsCount > 0)
            {
                InvokeRepeating(nameof(UpdateCoins), 0f, waitTimeOfClip / coinsCount);
            }
            else
            {
                CancelInvoke(nameof(UpdateCoins));
                gameOver_coins.text = Mathf.RoundToInt(coinsCount).ToString();
            }
        }

        void UpdateClipTime()
        {
            tempwaitTime++;

            if (tempwaitTime >= waitTimeOfClip)
            {
                CancelInvoke(nameof(UpdateCoins));
                CancelInvoke(nameof(UpdateClipTime));
                CancelInvoke(nameof(UpdateScore));

                gameOver_score.text = Mathf.RoundToInt(tempScore).ToString();
                gameOver_coins.text = Mathf.RoundToInt(coinsCount).ToString();

                replayButton.gameObject.SetActive(true);
                replayBtn.enabled = false;
                replayButton.SetTrigger("idle");
            }
        }

        public void PlayerStandAnimation()
        {
            StartCoroutine(ResultPlayerAnimation());
        }

        IEnumerator ResultPlayerAnimation()
        {
            _skeletonAnimResult.AnimationState.AddAnimation(0, "idle_open_mouth", true, 0);
            
            yield return new WaitForSeconds(0.33f);

            playerLandedParticleRight.Play();
            playerLandedParticleLeft.Play();
            SoundManager.instance.resultPlayer_conffetti_AudioSource.Play();
            SoundManager.instance.resultPlayer_yeah_AudioSource.Play();

            yield return new WaitForSeconds(0.3f);

            AfterScoreAndCoinAnim();
        }

        void UpdateScore()
        {
            tempScoreAnim++;
            gameOver_score.text = Mathf.RoundToInt(tempScoreAnim).ToString();
        }

        void UpdateCoins()
        {
            tempCoinAnim++;
            gameOver_coins.text = Mathf.RoundToInt(tempCoinAnim).ToString();
        }

        void AfterScoreAndCoinAnim()
        {
            StartCoroutine(GameOverScoreAnim());
        }

        public IEnumerator GameOverScoreAnim()
        {
            PlayerPrefsManager.CurrentScore = tempScore;
            PlayerPrefsManager.Coins = coinsCount;

            if (tempScore > PlayerPrefsManager.HighScore)
            {
                PlayerPrefsManager.HighScore = tempScore;
                gameOverTrophy.SetActive(true);
                HighScoreImageShow();
                yield return new WaitForSeconds(2.5f);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }

            backHomeButton.interactable = true;
            replayBtn.enabled = true;

            if (coinMeterValue >= 1)
            {
                prizebox.SetActive(true);
                prizeboxBg.SetActive(true);
                resultbg.SetActive(false);
            }
        }

        public void OnReplayClicked()
        {
            coinSpark.gameObject.SetActive(false);
            coinCollectAnimation.gameObject.SetActive(false);
            ResetCKDizzyAnim();
            StartAgain();
            GameManager.instance.GameRestartPlay();
            tempScoreAnim = 0;
            tempCoinAnim = 0;
            tempwaitTime = 0;
            waitTimeOfClip = 0;
            prizebox.SetActive(false);
            prizeboxBg.SetActive(false);

            SoundManager.instance.resultPlayer_conffetti_AudioSource.Stop();
            SoundManager.instance.resultPlayer_yeah_AudioSource.Stop();
        }

        public void HighScoreImageShow()
        {
            highScoreImage.GetComponent<AudioSource>().Play();
            highScoreConfetti.gameObject.SetActive(true);
            highScoreImage.transform.DOScale(1.75f, 0.3f).OnComplete(StopHighScoreAnim);
        }

        void StopHighScoreAnim()
        {
            highScoreImage.transform.DOScale(1.65f, 0.2f).OnComplete(HighScoreImageShowReset);
        }

        public void HighScoreImageShowReset()
        {
            StartCoroutine(ResetHighScoreText());
        }

        IEnumerator ResetHighScoreText()
        {
            yield return new WaitForSeconds(1.5f);
            highScoreConfetti.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            highScoreConfetti.Stop();
            highScoreImage.transform.DOScale(0.0f, 0.5f);
        }

        void ResetCKDizzyAnim()
        {
            _skeletonAnimResult.Skeleton.SetSkin(PlayerPrefsManager.SelectedPlayerName);
            _skeletonAnimResult.AnimationState.SetAnimation(0, "end_game", true);
        }

        public void TapToOpen()
        {
            StartCoroutine(TapToOpenFun());
        }

        public IEnumerator TapToOpenFun()
        {
            tapButton.interactable = false;
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[3], 1);
            //SoundManager.instance.PlayOneShot(gameAudioClip.open_chest);
            if (coinMeterValue >= 1)
            {
                prizelogicounter = PlayerPrefs.GetInt("prizeboxLogicCounter");
                unlockPlayerCounter = PlayerPrefs.GetInt("unlockPlayerCounter");
                unlockStickerCounter = PlayerPrefs.GetInt("unlockStickerCounter");

                if (prizelogicounter <= 0)
                {
                    prizelogicounter = 10;
                    PlayerPrefs.SetInt("prizeboxLogicCounter", prizelogicounter);
                    unlockPlayerCounter = 0;
                    PlayerPrefs.SetInt("unlockPlayerCounter", unlockPlayerCounter);
                    unlockStickerCounter = 0;
                    PlayerPrefs.SetInt("unlockStickerCounter", unlockStickerCounter);
                }
                if (prizelogicounter <= 10)
                {
                    prizelogicounter--;
                    PlayerPrefs.SetInt("prizeboxLogicCounter", prizelogicounter);
                    coinMeterValue--;
                    finalCoinScore.text = coinMeterValue.ToString();
                    PlayerPrefs.SetInt("coinMeterValue", coinMeterValue);
                    magicDecision = Random.Range(0, 1);

                    if (magicDecision == 0)
                    {
                        magicCounter = Random.Range(1, 10);
                        prizebox.SetActive(true);
                        prizeboxBg.SetActive(true);
                        resultbg.SetActive(false);
                        closePrizebox.SetActive(false);
                        openPrizebox.SetActive(true);
                        boxOpen.interactable = false;
                        //magicBoxResultBg.SetActive(false);
                        if (firstTimeOpen == 0)
                        {
                            _mystryBoxSpine.AnimationState.SetAnimation(0, "open", false);
                            _mystryBoxSpine.AnimationState.AddAnimation(0, "open_loop", true, 1f);
                            firstTimeOpen++;
                        }
                        else
                        {
                            _mystryBoxSpine.AnimationState.SetAnimation(0, "open_click", false);
                            _mystryBoxSpine.AnimationState.AddAnimation(0, "open_loop", true, 1f);
                        }

                        yield return new WaitForSeconds(0.67f);

                        if (magicCounter >= 1 && magicCounter <= 5)
                        {
                            if (unlockStickerCounter < 4)
                            {
                                UnlockStickers();
                                unlockStickerCounter++;
                                PlayerPrefs.SetInt("unlockStickerCounter", unlockStickerCounter);
                            }
                            else
                            {
                                UnlockCoins();
                            }

                            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[28]);
                        }

                        if (magicCounter >= 6 && magicCounter <= 10)
                        {
                            if (unlockPlayerCounter < 1)
                            {
                                UnlockPlayer();
                                unlockPlayerCounter++;
                                PlayerPrefs.SetInt("unlockPlayerCounter", unlockPlayerCounter);
                            }
                            else
                            {
                                UnlockCoins();
                            }
                            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[28]);
                        }
                    }

                    if (magicDecision == 1)
                    {
                        UnlockCoins();
                        SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[28]);
                    }

                    boxOpen.interactable = true;
                    prizebgResult.SetActive(true);
                    startGlow.gameObject.SetActive(true);
                    startGlow.Play();
                }
            }
            else
            {
                prizebox.SetActive(false);
                prizeboxBg.SetActive(false);
                resultbg.SetActive(true);
            }
            //magicBoxResultBg.SetActive(false);
            startGlow.gameObject.SetActive(true);
            startGlow.Play();
            yield return new WaitForSeconds(0.5f);
            tapButton.interactable = true;
        }

        public void TapToContinue()
        {
            StartCoroutine(TapToContinueAnim());
        }

        public IEnumerator TapToContinueAnim()
        {
            //magicBoxResultBg.SetActive(true);
            startGlow.gameObject.SetActive(true);
            resultbg.SetActive(false);
            startGlow.gameObject.SetActive(false);
            if (coinMeterValue >= 1)
            {
                prizebox.SetActive(true);
                prizeboxBg.SetActive(true);
                resultbg.SetActive(false);
                _mystryBoxSpine.AnimationState.SetAnimation(0, "open_click", true);
                _mystryBoxSpine.AnimationState.AddAnimation(0, "open_loop", true, 1);
                for (int i = 0; i < stickersForUnlock.Count; i++)
                {
                    stickersForUnlock[i].SetActive(false);
                }

                for (int i = 0; i < playersForUnlock.Count; i++)
                {
                    playersForUnlock[i].SetActive(false);
                }
                coinsValueForPrizeBox.SetActive(false);

                yield return new WaitForSeconds(0.01f);
                StartCoroutine(TapToOpenFun());
            }
            else
            {
                prizebox.SetActive(false);
                prizeboxBg.SetActive(false);
                _mystryBoxSpine.AnimationState.SetAnimation(0, "wobble", true);
                firstTimeOpen = 0;
                resultbg.SetActive(true);
                StartCoroutine(CoinUpdateAnim());
            }
        }

        public void TapToMystryBox()
        {
            if (firstTimeOpen == 0)
            {
                TapToOpen();
                firstTimeOpen++;
            }
            else
            {
                prizebgResult.SetActive(false);
                TapToContinue();
            }
        }

        public IEnumerator CoinUpdateAnim()
        {
            if (getNewCoins > 0)
            {
                int newCoinData = PlayerPrefsManager.Coins + getNewCoins;
                while (PlayerPrefsManager.Coins < newCoinData)
                {
                    coinCollectAnimation.gameObject.SetActive(true);
                    coinSpark.gameObject.SetActive(true);
                    coinSpark.Play();
                    if (getNewCoins > 0 && getNewCoins < 48)
                    {
                        PlayerPrefsManager.Coins++;
                    }
                    else if (getNewCoins < 49)
                    {
                        PlayerPrefsManager.Coins += 5;
                    }
                    else if (getNewCoins > 50)
                    {
                        PlayerPrefsManager.Coins += 10;
                    }
                    gameOver_coins.text = PlayerPrefsManager.Coins.ToString();
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        public void UnlockPlayer()
        {
            List<GameObject> remainingPlayersList = new List<GameObject>();

            for (int i = 0; i < playersForUnlock.Count; i++)
            {
                //if (playersForUnlock[i].GetComponent<PlayerProperty>().isLocked)
                if (playersForUnlock[i].GetComponent<PlayerProperty>().GetPlayerStatus())
                {
                    remainingPlayersList.Add(playersForUnlock[i]);
                }
            }

            int randomPlayerIndex = 0;

            if (remainingPlayersList.Count > 0)
            {
                randomPlayerIndex = Random.Range(0, remainingPlayersList.Count);
                int index = playersForUnlock.FindIndex(x => x.name == remainingPlayersList[randomPlayerIndex].name);
                playersForUnlock[index].GetComponent<PlayerProperty>().SetPlayerUnlock();
                playersForUnlock[index].GetComponent<PlayerProperty>().isLocked = false;
                //PBPlayerPrefs.SetBool(playersForUnlock[index].name + ".lock", false);
                playersForUnlock[index].SetActive(true);

                Debug.Log(playersForUnlock[index].name);
                playersForUnlock[index].transform.DOScale(0.55f, 0.15f).OnComplete(PlayerReset);
            }
            else
            {
                UnlockCoins();
            }
        }

        public void UnlockStickers()
        {
            List<GameObject> remainingStickersList = new List<GameObject>();

            for (int i = 0; i < stickersForUnlock.Count; i++)
            {
                if (stickersForUnlock[i].GetComponent<StickerProperty>().GetStickerStatus())
                {
                    remainingStickersList.Add(stickersForUnlock[i]);
                }
            }

            int randomStickerIndex = 0;
            if (remainingStickersList.Count > 0)
            {
                randomStickerIndex = Random.Range(0, remainingStickersList.Count);
                int index = stickersForUnlock.FindIndex(x => x.name == remainingStickersList[randomStickerIndex].name);
                stickersForUnlock[index].GetComponent<StickerProperty>().SetStickerUnlock();
                stickersForUnlock[index].SetActive(true);
                //PBPlayerPrefs.SetBool(stickersForUnlock[index].name + ".png", true);
                stickersForUnlock[index].transform.DOScale(1.15f, 0.15f).OnComplete(StickerReset);
            }
            else
            {
                UnlockCoins();
            }
        }

        public void UnlockCoins()
        {
            whichCoinsRelaease = Random.Range(0, 5);
            coinsValueForPrizeBox.SetActive(true);
            coinsValueForPrizeBox.transform.DOScale(1.15f, 0.15f).OnComplete(CoinReset);

            switch (whichCoinsRelaease)
            {
                case 0:
                    coinValues.text = "5";
                    getNewCoins += 5;
                    break;
                case 1:
                    coinValues.text = "10";
                    getNewCoins += 10;
                    break;
                case 2:
                    coinValues.text = "15";
                    getNewCoins += 15;
                    break;
                case 3:
                    coinValues.text = "20";
                    getNewCoins += 20;
                    break;
            }
        }

        public void PlayerReset()
        {
            for (int i = 0; i < playersForUnlock.Count; i++)
            {
                playersForUnlock[i].transform.DOScale(0.45f, 0.15f);
            }
        }

        public void StickerReset()
        {
            for (int i = 0; i < stickersForUnlock.Count; i++)
            {
                stickersForUnlock[i].transform.DOScale(1f, 0.15f);
            }
        }

        public void CoinReset()
        {
            coinsValueForPrizeBox.transform.DOScale(1f, 0.15f);
        }
    }
