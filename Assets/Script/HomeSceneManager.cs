using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using System;


    public class HomeSceneManager : MonoBehaviour
    {
        public static HomeSceneManager instance;

        private float meterValue = 0.01f;
        [SerializeField] private TMP_Text setHighScore;
        [SerializeField] private TMP_Text localize_popupText;
        
        [SerializeField] private Animator spalshScreenAnim;

        [SerializeField] private string stickerGame = "Sticker";
        [SerializeField] private string unloadCkGO = "unloadCkGO";

        [SerializeField] private Button playButton, stickerButton, backButton;
        [SerializeField] private List<GameObject> logoImages = new();

        public GameObject lockPopUp; 
        public GameObject lockpopupChild;

        //public GameObject debugger
        //public TMP_Text setGameSpeedTxt, meterText;
        //public Slider speedSlider;
        //public Slider coinmeterSlider;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            HomeSceneToLoad();
        }

        void LoadLogosSetActiveFalse()
        {
            for (int i = 0; i < logoImages.Count; i++)
            {
                logoImages[i].SetActive(false);
            }
        }

       

        public void HomeSceneToLoad()
        {
            if (PlayerPrefsManager.SetMeterSpeed == 0)
            {
                meterValue = 0.01f;
                PlayerPrefsManager.SetMeterSpeed = meterValue;
            }

            setHighScore.text = PlayerPrefsManager.HighScore.ToString();
            EnableButtons();
            //blueScreenObject.SetActive(false);
            spalshScreenAnim.gameObject.SetActive(true);
            spalshScreenAnim.SetTrigger("FadeIn");
        }

        public void EnableButtons()
        {
            playButton.interactable = true;
            stickerButton.interactable = true;
            backButton.interactable = true;
        }

        public void DisableButtons()
        {
            playButton.interactable = false;
            stickerButton.interactable = false;
            backButton.interactable = false;
        }

        //public void DebugerOff()
        //{
        //    debugger.transform.DOScale(0f, 0.75f);
        //}

        //public void DebugerOn()
        //{
        //    debugger.transform.DOScale(1f, 0.75f);
        //}

        //public void OnMeterCoinMeterFun()
        //{
        //    float y;
        //    y = Mathf.Floor(coinmeterSlider.value * 10) / 10;
        //    meterValue = y;
        //    PlayerPrefsManager.SetMeterSpeed = y;
        //    meterText.text = meterValue.ToString();
        //}

        public void LoadToStickerGame()
        {
            spalshScreenAnim.gameObject.SetActive(true);
            spalshScreenAnim.SetTrigger("FadeOut");
            //SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[0], 1);
            SoundManager.instance.PlayOneShotClip(HomeScene_gameAudioClips.OnClickSticketLoadBtn);
            DisableButtons();
            StartCoroutine(WaitForProgram());
        }

        IEnumerator WaitForProgram()
        {
            yield return new WaitForSeconds(.1f);
            StartCoroutine(LoadAsyncScene());
        }

        IEnumerator LoadAsyncScene()
        {
            //Screen.orientation = ScreenOrientation.LandscapeLeft;
            Resources.UnloadUnusedAssets();
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(stickerGame);
            asyncLoad.allowSceneActivation = false;
            yield return (asyncLoad.progress > 0.9f);
            StartCoroutine(Loaded(asyncLoad));
        }

        IEnumerator Loaded(AsyncOperation sync)
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.UnloadSceneAsync("MainMenu");
            sync.allowSceneActivation = true;
        }

        public void LoadToManiGame()
        {
            Time.timeScale = 1;
            //SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[0], 1);
            SoundManager.instance.PlayOneShotClip(HomeScene_gameAudioClips.OnClickSticketLoadBtn);
            //blueScreenObject.SetActive(true);
            //StartCoroutine(BackTomainGame());
            LoadToUnloadCkGOScene();
        }

        public void LoadToUnloadCkGOScene()
        {
            SceneManager.LoadScene(unloadCkGO);
        }

//        IEnumerator BackTomainGame()
//        {
//            yield return new WaitForSeconds(0.1f);
//            AssetBundleManager.instance.UnloadBundle();
//#if KIDDOPIA
//SceneManager.LoadScene("ExitScreen");
//#endif
//            SoundManager.DestroyInstance();
//            Resources.UnloadUnusedAssets();
//        }

        public void ClearAllSave()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("isFirst", 0);
            SceneManager.LoadScene("ckgoGame");
        }

        public void OnMouseDownEnter(Button buttonDown)
        {
            buttonDown.transform.DOScale(1.15f, 0.15f); 
        }

        public void OnMouseDownExit(Button buttonExit)
        {
            buttonExit.transform.DOScale(1.0f, 0.15f);
        }
    }
