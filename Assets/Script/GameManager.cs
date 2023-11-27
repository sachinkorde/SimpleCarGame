using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Spine.Unity;
using DG.Tweening;

    public class GameManager : MonoBehaviour
    {
        public enum GameStates
        {
            PreGameStart,
            GameStart,
            GameOver,
            Pause,
            Resume
        }

        public static GameManager instance;

        public GameStates currentState;

        [SerializeField] private GameObject pausePopup, pausePopupPlayButton, pausePopupHomeButton;
        public GameObject player, playerMagnet, playerBooster, startresumeAnim, coinCollectionAnim; //popUpExit //resumePopUp
        public GameObject coinMeterMagicBox, coinMeterMagicBoxBg, coinMeterHighLightBg, coinMeterStarBurst, coinMeterduplicateBox, coinmMeterAnim, dummycoinMeterProgress, coinMeterTextHolder; //coinMeterMagicBox1

        public Image healthBgImage, healthBarImage, coinMeterProgess;

        public Animator boosterTitleImage, shieldTitleImage, magnetTitleImage, twoXTitleImage;

        public ParticleSystem shieldPlayer;
        public ParticleSystem powerupsParticle;
        public ParticleSystem healthBarParticle;

        public SkeletonAnimation _skeletonAnimPlayer;

        [SerializeField] private float boostpower = 15.0f;
        [SerializeField] private float shieldPower = 15.0f;
        [SerializeField] private float magnetPower = 15.0f;
        [SerializeField] private float xPower = 15.0f;
        [SerializeField] private float amountToFill = 0.075f;
        [SerializeField] private float amountToFillCoinMeter = 0.013f;

        public float score = 0;
        public float boosterSpeed = 0.0f;
        public float objectSpeed = 6.25f;

        public bool isBoost = false;
        public bool isGameOver = false;
        public bool isShield = false;
        public bool isMagnetPower = false;
        public bool is2xPower = false;

        [SerializeField] private GameObject[] bgMoving;

        [SerializeField] private string startBooster = "startBooster";
        //[SerializeField] private string game = "Game";
        [SerializeField] private string mainMenu = "MainMenu";

        public Animator coinMeterMain;
        public Animator meter;
        public AudioSource meterAudio;
        //int tempCounter = 0;
        //public Button debuggerForMuteBGM;

        bool isPaused = false;

        void OnApplicationPause()
        {
            if (isPaused)
            {
                PausePopupOn();
            }
        }

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            Application.targetFrameRate = 60;
            currentState = GameStates.PreGameStart;
            _skeletonAnimPlayer = player.GetComponent<SkeletonAnimation>();
            isPaused = false;
            player.SetActive(true);
            _skeletonAnimPlayer.Skeleton.SetSkin(PlayerPrefsManager.SelectedPlayerName);
            Time.timeScale = 1;

            GameStartPlay();
        }

        //public void MuteBGMDebugger()
        //{
        //    if(tempCounter == 0)
        //    {
        //        SoundManager.instance.gameBgSound.Stop();
        //        debuggerForMuteBGM.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "UnMute BGM";
        //        tempCounter++;
        //    }
        //    else
        //    {
        //        tempCounter = 0;
        //        SoundManager.instance.gameBgSound.Play();
        //        debuggerForMuteBGM.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Mute BGM";
        //    }
            
        //}

        //public void DupliCateCoinMeterAnim()
        //{
        //    StartCoroutine(DupliCateCoinMeterIenumerator());
        //}

        //public IEnumerator DupliCateCoinMeterIenumerator()
        //{
        //    coinMeterduplicateBox.SetActive(true);
        //    yield return new WaitForSeconds(1f);
        //    //coinMeterMagicBox1.transform.DOScale(1.0f, 0.3f);
        //}

        public void OnEndAllCoinMeterAnim()
        {
            ScoreManager.instance.coinMeterValue++;
            ScoreManager.instance.coinMeterText.text = ScoreManager.instance.coinMeterValue.ToString();
            PlayerPrefs.SetInt("coinMeterValue", ScoreManager.instance.coinMeterValue);
        }

        public void ChangeAnimCoinMeterProgess()
        {
            if (coinMeterProgess.fillAmount == 1.0f)
            {
                coinMeterMain.SetTrigger("coinMeterAnim");
                coinMeterProgess.fillAmount = 0;
            }
        }

        public void CoinCollectionAnimFun()
        {
            StartCoroutine(CoinCollectiomAnim());
        }

        public IEnumerator CoinCollectiomAnim()
        {
            coinCollectionAnim.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            coinCollectionAnim.SetActive(false);
        }

        public void CoinMeterFun()
        {
            if (coinMeterProgess.fillAmount < 1)
            {
                //coinMeterProgess.fillAmount += amountToFillCoinMeter;

                coinMeterProgess.DOFillAmount(coinMeterProgess.fillAmount + amountToFillCoinMeter, 0.3f);
            }
            PlayerPrefsManager.GemsFillAmount = coinMeterProgess.fillAmount;
            //amountToFillCoinMeter = PlayerPrefsManager.GemsFillAmount;
        }

        bool sameWhichPowerup = false;

        public void PowerUpsStart(bool whichPowerUp)
        {
            meter.gameObject.SetActive(true);
            //powerupsParticle.gameObject.SetActive(true);
            
            if (whichPowerUp == isBoost)
            {
                boosterTitleImage.transform.DOScale(1.15f, 0.35f).OnComplete(OnStartPowerUpTitlesAnim);
            }
            else if (whichPowerUp == isMagnetPower)
            {
                magnetTitleImage.transform.DOScale(1.15f, 0.35f).OnComplete(OnStartPowerUpTitlesAnim);
            }
            else if (whichPowerUp == isShield)
            {
                shieldTitleImage.transform.DOScale(1.15f, 0.35f).OnComplete(OnStartPowerUpTitlesAnim);
            }
            else if (whichPowerUp == is2xPower)
            {
                twoXTitleImage.transform.DOScale(1.15f, 0.35f).OnComplete(OnStartPowerUpTitlesAnim);
            }

            sameWhichPowerup = whichPowerUp;

            Invoke(nameof(PowerUpParticlePlay), 0.075f);
            Invoke(nameof(ResetPowerUp), 0.35f);
        }

        void ResetPowerUp()
        {
            if (sameWhichPowerup == isBoost)
            {
                boosterTitleImage.transform.DOScale(1.0f, 0.15f);
            }
            else if (sameWhichPowerup == isMagnetPower)
            {
                magnetTitleImage.transform.DOScale(1.0f, 0.15f);
            }
            else if (sameWhichPowerup == isShield)
            {
                shieldTitleImage.transform.DOScale(1.0f, 0.15f);
            }
            else if (sameWhichPowerup == is2xPower)
            {
                twoXTitleImage.transform.DOScale(1.0f, 0.15f);
            }
        }

        void PowerUpParticlePlay()
        {
            powerupsParticle.Play();
        }

        public void OnStartPowerUpTitlesAnim()
        {
            healthBgImage.transform.DOScaleX(1.0f, 0.35f);
            Debug.Log("here start aim");
            Invoke(nameof(HealthBarImageAnim), 0.15f);
            //healthBarParticle.gameObject.SetActive(true);
            Invoke(nameof(ParticlePlayForHealthBar), 0.2f);
        }

        void HealthBarImageAnim()
        {
            healthBarImage.transform.DOScaleX(1.0f, 0.55f);
        }

        void ParticlePlayForHealthBar()
        {
            healthBarParticle.Play();
        }

        public void OnEndPowerUpTitlesAnim()
        {
            healthBgImage.transform.DOScaleX(0.0f, 0.5f).SetUpdate(true).OnComplete(OnEndAnimTitles);
            healthBarImage.transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
            //healthBarImage.transform.DOScaleX(0.0f, 0.5f).SetUpdate(true);
            //healthBarParticle.gameObject.SetActive(false);
            //healthBarParticle.Stop();

            if (meterAudio.isPlaying)
            {
                meterAudio.loop = false;
                meterAudio.Stop();
            }
        }

        void OnEndAnimTitles()
        {
            boosterTitleImage.gameObject.transform.DOScale(0f, 0.35f).SetUpdate(true);
            shieldTitleImage.gameObject.transform.DOScale(0f, 0.35f).SetUpdate(true);
            magnetTitleImage.gameObject.transform.DOScale(0f, 0.35f).SetUpdate(true);
            twoXTitleImage.gameObject.transform.DOScale(0f, 0.35f).SetUpdate(true);
            meter.SetTrigger("idle");
            
            healthBgImage.transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
            healthBarImage.transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
            healthBarImage.fillAmount = 1.0f;
            //powerupsParticle.gameObject.SetActive(false);
            //powerupsParticle.Stop();
        }

        public void ResetMeterValues()
        {
            healthBarImage.fillAmount = 1f;
            boosterTitleImage.gameObject.transform.DOScale(0f, 1.0f);
            shieldTitleImage.gameObject.transform.DOScale(0f, 1.0f);
            magnetTitleImage.gameObject.transform.DOScale(0f, 1.0f);
            twoXTitleImage.gameObject.transform.DOScale(0f, 1.0f);
            healthBgImage.transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
            healthBarImage.transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
        }

        private void Update()
        {
            OnClicedNativeBack();
            switch (currentState)
            {
                case GameStates.PreGameStart:

                    if (!isGameOver)
                    {
                        score = 0;
                        Screen.orientation = ScreenOrientation.Portrait;
                        StopAllCoroutines();
                        coinMeterProgess.fillAmount = PlayerPrefsManager.GemsFillAmount;
                        ScoreManager.instance.StartAgain();
                        PlayerDiedAnimations.instance.PreRestartGame();
                        GamePlay.instance.PreRestartGame();
                        PreRestartGame();
                    }

                    break;

                case GameStates.GameStart:

                    Time.timeScale = 1;
                    ScoreManager.instance.tempScore = (int)score;
                    ScoreManager.instance.saveScore.text = "" + ScoreManager.instance.tempScore;

                    if (isBoost)
                    {
                        playerBooster.SetActive(true);
                        boostpower -= Time.deltaTime;
                        healthBarImage.fillAmount -= amountToFill * Time.deltaTime;
                        score += Time.deltaTime * 100;
                        GamePlay.instance.isSpawnPowerUp = true;
                    }
                    else
                    {
                        if (GamePlay.instance.firstCount == 1)
                        {
                            score += Time.deltaTime * 20;
                        }
                    }

                    if (boostpower <= 0)
                    {
                        isBoost = false;
                        _skeletonAnimPlayer.timeScale = 1.0f;
                        GamePlay.instance.isSpawnPowerUp = false;
                        playerBooster.SetActive(false);
                        boostpower = 15.0f;
                        boosterSpeed -= Time.deltaTime * 0.01f;
                        healthBarImage.fillAmount = 1.0f;

                        if (boosterSpeed >= 0.1f)
                        {
                            boosterSpeed = 2.0f;
                        }
                    }

                    if (isShield)
                    {
                        shieldPower -= Time.deltaTime;
                        GamePlay.instance.isSpawnPowerUp = true;
                        shieldPlayer.gameObject.SetActive(true);
                        shieldPlayer.Play();
                        healthBarImage.fillAmount -= amountToFill * Time.deltaTime;
                    }

                    if (shieldPower <= 0)
                    {
                        isShield = false;
                        GamePlay.instance.isSpawnPowerUp = false;
                        shieldPlayer.gameObject.SetActive(false);
                        shieldPlayer.Stop();
                        shieldPower = 15;
                        healthBarImage.fillAmount = 1.0f;
                    }

                    if (isMagnetPower)
                    {
                        playerMagnet.SetActive(true);
                        GamePlay.instance.isSpawnPowerUp = true;
                        magnetPower -= Time.deltaTime;
                        healthBarImage.fillAmount -= amountToFill * Time.deltaTime;
                    }

                    if (magnetPower <= 0)
                    {
                        isMagnetPower = false;
                        GamePlay.instance.isSpawnPowerUp = false;
                        playerMagnet.SetActive(false);
                        magnetPower = 15.0f;
                        healthBarImage.fillAmount = 1.0f;
                    }

                    if (is2xPower)
                    {
                        xPower -= Time.deltaTime;
                        GamePlay.instance.isSpawnPowerUp = true;
                        healthBarImage.fillAmount -= amountToFill * Time.deltaTime;
                    }

                    if (xPower <= 0)
                    {
                        is2xPower = false;
                        GamePlay.instance.isSpawnPowerUp = false;
                        healthBarImage.fillAmount = 1.0f;
                        xPower = 15;
                    }

                    if (healthBarImage.fillAmount < 0.17f)
                    {
                        meter.SetTrigger(startBooster);
                    }
                    else
                    {
                        meter.SetTrigger("idle");
                    }

                    break;

                case GameStates.GameOver:
                    if (!isGameOver)
                    {
                        isGameOver = true;
                        ScoreManager.instance.replayButton.transform.localScale = Vector3.zero;
                        ScoreManager.instance._skeletonAnimResult.AnimationState.AddEmptyAnimation(0, 0, 0);
                        ScoreManager.instance._skeletonAnimResult.AnimationState.SetAnimation(0, "end_game", false);
                        ScoreManager.instance.GameOver();
                        //score = 0;
                    }
                    break;
            }
        }

        public void PreRestartGame()
        {
            is2xPower = false;
            isBoost = false;
            isGameOver = false;
            isMagnetPower = false;
            isShield = false;

            boostpower = 15;
            shieldPower = 15;
            magnetPower = 15;
            xPower = 15;

            ResetMeterValues();
            for (int i = 0; i < player.transform.childCount; i++)
            {
                player.transform.GetChild(i).gameObject.SetActive(false);
            }

            pausePopup.SetActive(false);
            //resumePopUp.SetActive(false);
            meter.gameObject.SetActive(true);
            PlayerDiedAnimations.instance.PreRestartGame();
            GamePlay.instance.PreRestartGame();
        }

        public void GameStartPlay()
        {
            //SceneHandler.instance.LoadGameScene();
            PreRestartGame();
            ScoreManager.instance.StartAgain();
            currentState = GameStates.GameStart;
            GamePlay.instance.LoadGamePlay();
            coinMeterProgess.fillAmount = PlayerPrefsManager.GemsFillAmount;
        }

        public void GameRestartPlay()
        {
            SoundManager.instance.aud.Stop();
            GamePlay.instance.firstCount = 0;
            GamePlay.instance.intialSpawnCounter = 0;
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[7], 1);
            //SoundManager.instance.PlayOneShot(gameAudioClip.blip);
            PreRestartGame();
            currentState = GameStates.GameStart;
            GamePlay.instance.LoadGamePlay();
            player.SetActive(true);
            _skeletonAnimPlayer.Skeleton.SetSkin(PlayerPrefsManager.SelectedPlayerName);
            PlayerControllerLoad();
            score = 0;
            ScoreManager.instance.coinsCount = 0;
            coinMeterProgess.fillAmount = PlayerPrefsManager.GemsFillAmount;


            Debug.Log(PlayerPrefsManager.GemsFillAmount);
            Debug.Log(coinMeterProgess.fillAmount);
        }

        public void PlayerControllerLoad()
        {
            PlayerController.instance.LoadPlayerController();
        }

        public void PausePopupOn()
        {
            isPaused = true;
            if (meterAudio.isPlaying)
            {
                meterAudio.Pause();
            }

            if (SoundManager.instance.aud.isPlaying)
            {
                SoundManager.instance.aud.Pause();
            }
            ScoreManager.instance.pauseButton.interactable = false;
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[7], 1);
            //SoundManager.instance.PlayOneShot(gameAudioClip.blip);
            currentState = GameStates.Pause;
            StopAllCoroutines();
            Time.timeScale = 0;
            pausePopup.SetActive(true);
        }

        public void OnPlayButtonClickOnPause()
        {
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[7], 1);
            //SoundManager.instance.PlayOneShot(gameAudioClip.blip);
            ResumeGame();
        }

        public void ResumeGame()
        {
            isPaused = false;
            ScoreManager.instance.pauseButton.interactable = false;
            pausePopup.SetActive(false);
            //popUpExit.SetActive(false);
            pausePopupPlayButton.transform.localScale = Vector3.one;
            pausePopupHomeButton.transform.localScale = Vector3.one;
            //resumePopUp.SetActive(true);
            startresumeAnim.SetActive(true);
            ResumeAnimatiom resumeAnimation = startresumeAnim.GetComponent<ResumeAnimatiom>();
            //resumeAnimation.OnAnimComplete += StartDelay;
        }

        public void StartDelay()
        {
            //resumePopUp.SetActive(false);
            startresumeAnim.SetActive(false);
            currentState = GameStates.GameStart;
            ScoreManager.instance.pauseButton.interactable = true;
            meterAudio.UnPause();
            SoundManager.instance.aud.UnPause();
        }

        public IEnumerator OnGameLoadAnimation()
        {
            //resumePopUp.SetActive(true);
            startresumeAnim.SetActive(true);
            yield return new WaitForSeconds(2);
            //resumePopUp.SetActive(false);
            startresumeAnim.SetActive(false);
        }

        public void LoadToHomeScene()
        {
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[0], 1);
            //SoundManager.instance.PlayOneShot(gameAudioClip.camera_click);
            SceneHandler.instance.LoadHomeScene();
            currentState = GameStates.PreGameStart;
        }

        public void BackTomainMenu()
        {
            Time.timeScale = 1;
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[0], 1);
            //SoundManager.instance.PlayOneShot(gameAudioClip.camera_click);
            UnityEngine.Resources.UnloadUnusedAssets();
            SceneManager.LoadScene(mainMenu);
        }

        public void OnMouseDownEnter(Button buttonDown)
        {
            if (buttonDown.interactable == true)
            {
                //buttonDown.transform.localScale = new Vector2(1.15f, 1.15f);
                buttonDown.transform.DOScale(1.15f, 0.15f).SetUpdate(true);
            }
        }

        public void OnMouseDownExit(Button buttonExit)
        {
            if (buttonExit.interactable == true)
            {
                //buttonExit.transform.localScale = new Vector2(1f, 1f);
                buttonExit.transform.DOScale(1.0f, 0.15f).SetUpdate(true);
            }
        }

        public void OnClicedNativeBack()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
            {
                Debug.Log("Pressed Escape");
                if (PlayerPrefs.GetString("LoadGame") == "Gamescene")
                {
                    if (GamePlay.instance.firstCount == 1)
                    {
                        ScoreManager.instance.pauseButton.interactable = false;
                        currentState = GameStates.Pause;
                        StopAllCoroutines();
                        Time.timeScale = 0;
                        //popUpExit.SetActive(true);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    Application.Quit();
                }
            }
        }
    
}
