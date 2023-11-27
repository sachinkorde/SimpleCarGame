using System.Collections;
using UnityEngine;
using Spine.Unity;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


    public class PlayerCarousel : MonoBehaviour
    {
        public static PlayerCarousel instance;

        public SkeletonGraphic[] players;

        public RectTransform rect;

        public float playerWidth;
        public float player_gap = -845f;
        private float lerpTimer;
        private float lerpPosition;
        private float mousePositionStartX;
        private float mousePositionEndX;
        private float dragAmount;
        private float screenPosition;
        private float lastScreenPosition;
        [SerializeField] private float upperScreenLimit = 300;
        [SerializeField] private float lowerScreenLimit = 1600;

        public int swipeThrustHold = 30;
        private int m_currentIndex;

        public bool isSwipe;
        public bool isPopUp = false;

        //public TMP_Text carouselIndex;

        public GameObject spalshScreen, playButtonParent;

        [SerializeField] private Button playbutton;

        [SerializeField] private string fly = "fly";
        [SerializeField] private string giggle = "giggle";
        [SerializeField] private string heyy = "heyy";
        [SerializeField] private string idle_open_mouth = "idle_open_mouth";

        public int CurrentIndex { get { return m_currentIndex; } }

        #region mono
        void Awake()
        {
            instance = this;
            PlayerCarouselAnimation();
        }

        private void Start()
        {
            HomeSceneManager.instance.DisableButtons();
        }

        //private void Start()
        //{
        //    SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[21], 1f);
        //}

        public void PlayerCarouselAnimation()
        {
            if (PlayerPrefs.GetInt("isFirst") == 0)
            {
                PlayerPrefs.SetInt("isFirst", 1);
                PlayerPrefs.SetInt("scrollpos", 4);
            }

            StartCoroutine(GoToIndexAnim());
            playerWidth = rect.rect.width;

            for (int i = 1; i < players.Length; i++)
            {
                players[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(((playerWidth + player_gap) * i), 0);
            }
        }

        public void SetPlayerGapAccordingToScreen()
        {
            if (Screen.width > 1350)
            {
                player_gap = -900;
            }
            else
            {
                player_gap = -950;
            }
        }

        void Update()
        {
            //UpdateCurrentIndexLable();
            UpdateCarouselView();
        }
        #endregion

        #region private methods
        //void UpdateCurrentIndexLable()
        //{
        //    if (carouselIndex)
        //    {
        //        carouselIndex.text = m_currentIndex.ToString();
        //    }
        //}

        void UpdateCarouselView()
        {
            if (Screen.width < 1350)
            {
                upperScreenLimit = 400;
                lowerScreenLimit = 1600;
            }
            lerpTimer += Time.deltaTime;
            if (lerpTimer < 0.333f)
            {
                screenPosition = Mathf.Lerp(lastScreenPosition, lerpPosition * -1, lerpTimer * 3);
                lastScreenPosition = screenPosition;
            }

            if (Input.GetMouseButtonDown(0) && Input.mousePosition.y > (Screen.height - lowerScreenLimit) && Input.mousePosition.y < (Screen.height - upperScreenLimit))
            {
                isSwipe = true;
                mousePositionStartX = Input.mousePosition.x;
            }

            if (Input.GetMouseButton(0))
            {
                if (!isPopUp)
                {
                    if (isSwipe)
                    {
                        mousePositionEndX = Input.mousePosition.x;
                        dragAmount = mousePositionEndX - mousePositionStartX;
                        screenPosition = lastScreenPosition + dragAmount;
                    }
                }
            }

            if (Mathf.Abs(dragAmount) > swipeThrustHold && isSwipe)
            {
                isSwipe = false;
                lastScreenPosition = screenPosition;
                if (m_currentIndex < players.Length)
                {
                    OnSwipeComplete();
                }
                else if (m_currentIndex == players.Length && dragAmount < 0)
                {
                    lerpTimer = 0;
                }
                else if (m_currentIndex == players.Length && dragAmount > 0)
                {
                    OnSwipeComplete();
                }
            }


            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(screenPosition + ((playerWidth + player_gap) * i), 0);
                if (Screen.width > 1350)
                {
                    if (i == m_currentIndex)
                    {
                        players[i].transform.localScale = Vector3.Lerp(players[i].transform.localScale, new Vector3(0.5f, 0.5f, 1f), Time.deltaTime * 5);
                    }
                    else
                    {
                        players[i].transform.localScale = Vector3.Lerp(players[i].transform.localScale, new Vector3(0.25f, 0.25f, 1f), Time.deltaTime * 5);
                    }
                }
                else
                {
                    if (i == m_currentIndex)
                    {
                        players[i].transform.localScale = Vector3.Lerp(players[i].transform.localScale, new Vector3(0.45f, 0.45f, 1f), Time.deltaTime * 5);
                    }
                    else
                    {
                        players[i].transform.localScale = Vector3.Lerp(players[i].transform.localScale, new Vector3(0.3f, 0.3f, 1f), Time.deltaTime * 5);
                    }
                }
            }
        }

        public void RandomAnimation()
        {
            int randomAnim = Random.Range(0, 3);
            
            switch (randomAnim)
            {
                case 0:
                    players[m_currentIndex].GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, fly, false);
                    players[m_currentIndex].GetComponent<SkeletonGraphic>().AnimationState.AddAnimation(0, idle_open_mouth, true, 0);
                    break;

                case 1:
                    players[m_currentIndex].GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, giggle, false);
                    players[m_currentIndex].GetComponent<SkeletonGraphic>().AnimationState.AddAnimation(0, idle_open_mouth, true, 0);
                    break;

                case 2:
                    players[m_currentIndex].GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, heyy, false);
                    players[m_currentIndex].GetComponent<SkeletonGraphic>().AnimationState.AddAnimation(0, idle_open_mouth, true, 0);
                    break;
            }
        }

        void OnSwipeComplete()
        {
            lastScreenPosition = screenPosition;

            if (dragAmount > 0)
            {
                if (dragAmount >= swipeThrustHold)
                {
                    if (m_currentIndex == 0)
                    {
                        lerpTimer = 0; lerpPosition = 0;
                    }
                    else
                    {
                        m_currentIndex--;
                        RandomAnimation();
                        //SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[4], 1.0f);
                        SoundManager.instance.PlayOneShotClip(HomeScene_gameAudioClips.swiprLeft);

                        lerpTimer = 0;
                        if (m_currentIndex < 0)
                            m_currentIndex = 0;
                        lerpPosition = (playerWidth + player_gap) * m_currentIndex;
                    }
                }
                else
                {
                    lerpTimer = 0;
                }
            }
            else if (dragAmount < 0)
            {
                if (Mathf.Abs(dragAmount) >= swipeThrustHold)
                {
                    if (m_currentIndex == players.Length - 1)
                    {
                        lerpTimer = 0;
                        lerpPosition = (playerWidth + player_gap) * m_currentIndex;
                    }
                    else
                    {
                        lerpTimer = 0;
                        m_currentIndex++;
                        RandomAnimation();
                        //SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[5], 1.0f);
                        SoundManager.instance.PlayOneShotClip(HomeScene_gameAudioClips.swipeRight);
                        lerpPosition = (playerWidth + player_gap) * m_currentIndex;
                    }
                }
                else
                {
                    lerpTimer = 0;
                }
            }
            dragAmount = 0;
        }
        #endregion

        public void OnPlayButtonClicked()
        {
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[7], 1);
            LoadToGame();
            HomeSceneManager.instance.DisableButtons();
        }

        public void LoadToGame()
        {
            if (!players[m_currentIndex].GetComponent<PlayerProperty>().isLocked)
            {
                switch (players[m_currentIndex].name)
                {
                    case "beachboy":
                        PlayerPrefsManager.SelectedPlayerName = "kman_beachboy";
                        break;

                    case "coolguy":
                        PlayerPrefsManager.SelectedPlayerName = "kman_cool_guy";
                        break;

                    case "cop":
                        PlayerPrefsManager.SelectedPlayerName = "kman_flying_cop";
                        break;

                    case "santa":
                        PlayerPrefsManager.SelectedPlayerName = "kman_flying_santa";
                        break;

                    case "default_char":
                        PlayerPrefsManager.SelectedPlayerName = "capkid";
                        break;

                    case "swag":
                        PlayerPrefsManager.SelectedPlayerName = "kman_flying_swag";
                        break;

                    case "ninja":
                        PlayerPrefsManager.SelectedPlayerName = "kman_ninja";
                        break;

                    case "punk":
                        PlayerPrefsManager.SelectedPlayerName = "kman_punk";
                        break;

                    default:
                        PlayerPrefsManager.SelectedPlayerName = "capkid";
                        break;
                }
                
                PlayerPrefs.SetInt("scrollpos", m_currentIndex);
                playButtonParent.transform.DOScale(1f, 0.1f);
                //GameManager.instance.GameStartPlay();
                UnityEngine.Resources.UnloadUnusedAssets();
                SceneManager.LoadScene("ckgoGame");
            }
            else
            {
                PopUpAnim();
            }
        }

        public void PopUpAnim()
        {
            isPopUp = true;
            playbutton.enabled = false;
            HomeSceneManager.instance.lockPopUp.SetActive(true);
            playButtonParent.transform.DOScale(1f, 0.1f);
            HomeSceneManager.instance.lockpopupChild.transform.DOScale(1f, 0.3f);
            SoundManager.instance.localize_unlock_characters.Play();
        }

        public void ClosePopUp()
        {
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[7], 1);
            HomeSceneManager.instance.lockpopupChild.transform.DOScale(0, 0.3f).OnComplete(DeActivateLockPopup);
            playButtonParent.transform.DOScale(1f, 0.1f);
            StopAllCoroutines();
            SoundManager.instance.localize_unlock_characters.Stop();
            HomeSceneManager.instance.EnableButtons();
        }

        void DeActivateLockPopup()
        {
            isPopUp = false;
            playbutton.enabled = true;
            HomeSceneManager.instance.lockPopUp.SetActive(false);
        }

        public IEnumerator SplashScreenAnim()
        {
            isPopUp = true;
            playbutton.enabled = false;
            spalshScreen.SetActive(true);
            float alpha = spalshScreen.GetComponent<Image>().color.a;
            while (alpha > 0)
            {
                alpha -= Time.deltaTime * 1;
                spalshScreen.GetComponent<Image>().color = new Color(spalshScreen.GetComponent<Image>().color.r,
                                                                     spalshScreen.GetComponent<Image>().color.g,
                                                                     spalshScreen.GetComponent<Image>().color.b,
                                                                     alpha);
                yield return new WaitForSeconds(0.01f);
            }
            spalshScreen.SetActive(false);
            yield return null;
        }

        public IEnumerator GoToIndexAnim()
        {
            StartCoroutine(SplashScreenAnim());
            yield return new WaitForSeconds(0.75f);
            GoToIndex(0);
            yield return new WaitForSeconds(0.25f);
            GoToIndexSmooth(PlayerPrefs.GetInt("scrollpos"));
            yield return new WaitForSeconds(1.0f);
            RandomAnimation();
            HomeSceneManager.instance.EnableButtons();
            yield return new WaitForSeconds(0.25f);
            playbutton.enabled = true;
        }

        #region public methods
        public void GoToIndex(int value)
        {
            m_currentIndex = value;
            lerpTimer = 0;
            lerpPosition = (playerWidth + player_gap) * m_currentIndex;
            screenPosition = lerpPosition * -1;
            lastScreenPosition = screenPosition;
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(screenPosition + ((playerWidth + player_gap) * i), 0);
            }
        }

        public void GoToIndexSmooth(int value)
        {
            m_currentIndex = value;
            lerpTimer = 0;
            lerpPosition = (playerWidth + player_gap) * m_currentIndex;
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[21]);
            SoundManager.instance.PlayOneShotClip(HomeScene_gameAudioClips.smoothPlayerSwipe);
            isPopUp = false;
            //playbutton.enabled = true;
        }
        #endregion
    }
