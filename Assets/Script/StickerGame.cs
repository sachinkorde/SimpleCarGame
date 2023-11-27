using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;


    public class StickerGame : MonoBehaviour
    {
        public static StickerGame instance;
        public static RaycastHit2D hit;

        GameObject go;
        public GameObject contentBg, bottomPanel, bottomButton, lockPopUp, lockpopupChild, deleteButton;
        public GameObject thisClone, stickerHolder, hideBottom, showBottom;
        public GameObject mainbg, dragParentHolder, stopClicks, StickerContent;
        public GameObject[] gameBoundaries;

        int clickValue = 0;

        public Animator setbottomAnim;
        public Animator splashScreenAnimator;

        public RootStickerlist stickerList = new RootStickerlist();

        public List<StickerlistData> stikersSave = new List<StickerlistData>();

        public Scrollbar bottomStickerScroll;
        public ScrollRect sticlerPanelScroll;

        public Canvas canvas;
        public Canvas canvasSavedDrag;

        public Button backButton;

        [SerializeField] private string newSticker = "NewSticker";
        [SerializeField] private string StickerData = "StickerData";
        [SerializeField] private string game = "ckgoGame";
        [SerializeField] private string mainMenu = "MainMenu";

        public AudioSource bottomPanelSource, bottomButtonSource;
        public AudioSource localize_unlock_these_items;

        private void Awake()
        {
            instance = this;
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }

        private void Start()
        {
            StickerLoad();
            dragParentHolder.SetActive(false);
            splashScreenAnimator.gameObject.SetActive(true);
            splashScreenAnimator.SetTrigger("FadeIn");
            stopClicks.SetActive(false);
        }

        public void StickerLoad()
        {
            Application.targetFrameRate = 60;
            StickerInstantiateAtBegining();
            LoadStickerData();
            backButton.interactable = true;
            bottomPanel.transform.localPosition = hideBottom.transform.localPosition;
        }

        public void BottomAnimation()
        {
            if (clickValue == 0)
            {
                while (Vector3.Distance(hideBottom.transform.localPosition, bottomPanel.transform.localPosition) > 0.01f)
                {
                    bottomPanel.transform.localPosition = Vector3.MoveTowards(bottomPanel.transform.localPosition, hideBottom.transform.localPosition, 25f * Time.deltaTime);
                }

                bottomButton.transform.DORotate(new Vector3(0f, 0f, -180f), 0.3f);
                bottomButtonSource.Play();
                bottomStickerScroll.value = 0.3f;
                clickValue++;
            }
            else if (clickValue == 1)
            {
                bottomStickerScroll.value = 0.3f;

                while (Vector3.Distance(showBottom.transform.localPosition, bottomPanel.transform.localPosition) > 0.01f)
                {
                    bottomPanel.transform.localPosition = Vector3.MoveTowards(bottomPanel.transform.localPosition, showBottom.transform.localPosition, 25f * Time.deltaTime);
                }

                StartCoroutine(ScrollAnim());
                bottomPanelSource.Play();
                bottomButton.transform.DORotate(new Vector3(0f, 0f, 0f), 0.3f);
                clickValue = 0;
            }
        }

        public IEnumerator ScrollAnim()
        {
            while (bottomStickerScroll.value > 0.0f)
            {
                bottomStickerScroll.value -= Time.deltaTime * 1;
                yield return new WaitForSeconds(0.01f);
            }
            yield return null;
        }

        public void LoadStickerData()
        {
            if (PlayerPrefs.GetString(StickerData) != "")
            {
                string data = PlayerPrefs.GetString(StickerData);
                stickerList = JsonUtility.FromJson<RootStickerlist>(data);
                if (stickerList.Slist.Count > 0)
                {
                    for (int i = 0; i < stickerList.Slist.Count; i++)
                    {
                        stikersSave.Add(stickerList.Slist[i]);
                        GameObject sticker = Instantiate(StickerContent.transform.GetChild(stikersSave[i].id - 1).gameObject, mainbg.transform);

                        for (int j = 0; j < sticker.transform.childCount; j++)
                        {
                            Destroy(sticker.transform.GetChild(j).gameObject);
                        }
                        Destroy(sticker.transform.GetComponent<Button>());
                        Destroy(sticker.GetComponent<StickerDragDrop>());
                        sticker.transform.gameObject.AddComponent<SavedStickerDragDrop>();
                        sticker.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                        sticker.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                        sticker.transform.localPosition = stikersSave[i].pos;
                        sticker.transform.localScale = stikersSave[i].scale;
                    }
                }
            }
        }

        public void SaveStickerData()
        {
            if(mainbg.transform.childCount > 0)
            {
                stikersSave.Clear();
                for (int i = 0; i < mainbg.transform.childCount; i++)
                {
                    StickerlistData t = new StickerlistData();
                    t.id = mainbg.transform.GetChild(i).gameObject.GetComponent<StickerProperty>().id;
                    t.pos = mainbg.transform.GetChild(i).localPosition;
                    t.scale = mainbg.transform.GetChild(i).localScale;
                    stikersSave.Add(t);
                }
                string temp = stikersSave.ToString();
                RootStickerlist ab = new RootStickerlist();
                ab.Slist = stikersSave;
                temp = JsonUtility.ToJson(ab);
                PlayerPrefs.SetString(StickerData, temp);
            }
        }

        public void PopUpAnim()
        {
            lockPopUp.SetActive(true);
            localize_unlock_these_items.Play();
            lockpopupChild.transform.DOScale(1f, 0.3f);
        }

        public void ClosePopUp()
        {
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[7], 1);
            lockpopupChild.transform.DOScale(0, 0.3f).OnComplete(DeActivateLockPopup);
            StopAllCoroutines();
            localize_unlock_these_items.Stop();
        }

        void DeActivateLockPopup()
        {
            lockPopUp.SetActive(false);
        }

        private void Update()
        {
            OnNativeBackClicked();
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.mousePosition)), Vector2.zero);
                if (hit.collider != null && !hit.transform.name.Contains("New") && !hit.transform.name.Contains("Clone"))
                {
                    if (hit.transform.GetComponent<StickerProperty>() && !hit.transform.GetComponent<StickerProperty>().isLocked)
                    {
                        go = Instantiate(hit.transform.gameObject, hit.transform);
                        if (go.transform.GetComponent<StickerDragDrop>() != null)
                        {
                            go.GetComponent<StickerDragDrop>().enabled = true;
                        }
                        if (go.transform.GetComponent<StickerProperty>() != null)
                        {
                            go.GetComponent<StickerProperty>().enabled = false;
                        }
                        for (int i = 0; i < go.transform.childCount; i++)
                        {
                            Destroy(go.transform.GetChild(i).gameObject);
                        }
                        Destroy(go.GetComponent<Button>());
                        go.transform.GetComponent<BoxCollider2D>().enabled = false;
                        go.transform.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                        go.transform.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                        go.transform.localPosition = Vector3.zero;
                        go.name = newSticker;
                        go.transform.localScale = Vector3.one;
                        SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[16], 1);
                    }
                }
            }
        }

        public void StickerInstantiateAtBegining()
        {
            for (int i = 0; i < StickerContent.transform.childCount; i++)
            {
                if (StickerContent.transform.GetChild(i).GetComponent<StickerProperty>() && !StickerContent.transform.GetChild(i).transform.GetComponent<StickerProperty>().isLocked)
                {
                    GameObject DuplicateGo = Instantiate(StickerContent.transform.GetChild(i).gameObject, StickerContent.transform.GetChild(i).transform);
                    DuplicateGo.GetComponent<StickerDragDrop>().enabled = true;
                    Destroy(DuplicateGo.transform.GetChild(0).gameObject);
                    Destroy(DuplicateGo.GetComponent<Button>());
                    DuplicateGo.transform.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                    DuplicateGo.transform.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                    DuplicateGo.transform.localPosition = Vector3.zero;
                    DuplicateGo.name = newSticker;
                    DuplicateGo.transform.localScale = Vector3.one;
                }
            }
        }

        public void LoadToHome()
        {
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[7], 1);
            stopClicks.SetActive(true);
            stopClicks.GetComponent<Image>().enabled = true;
            Screen.orientation = ScreenOrientation.Portrait;
            SaveStickerData();
            StartCoroutine(WaitForProgram());
        }

        IEnumerator WaitForProgram()
        {
            yield return new WaitForSeconds(.1f);
            StartCoroutine(LoadAsyncScene());
        }

        IEnumerator LoadAsyncScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mainMenu);
            asyncLoad.allowSceneActivation = false;
            yield return (asyncLoad.progress > 0.9f);

            StartCoroutine(Loaded(asyncLoad));
        }

        IEnumerator Loaded(AsyncOperation sync)
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.UnloadSceneAsync("Sticker");
            sync.allowSceneActivation = true;
        }

        public void BottomAnimAtStart()
        {
            StartCoroutine(SplashScreenAnim());
        }

        public IEnumerator SplashScreenAnim()
        {
            yield return new WaitForSeconds(0.2f);
            clickValue = 1;
            BottomAnimation();
            yield return new WaitForSeconds(0.1f);
            bottomButton.transform.DORotate(new Vector3(0f, 0f, -180f), 0.3f);
            bottomStickerScroll.value = 1.0f;
        }

        public void OnMouseDownEnter(Button buttonDown)
        {
            buttonDown.transform.DOScale(1.15f, 0.15f).SetUpdate(true);
        }

        public void OnMouseDownExit(Button buttonExit)
        {
            buttonExit.transform.DOScale(1.0f, 0.15f).SetUpdate(true);
        }

        public void OnNativeBackClicked()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(game);
            }
        }
    }

    [System.Serializable]
    public class StickerlistData
    {
        public int id;
        public Vector3 pos;
        public Vector3 scale;
    }

    [System.Serializable]
    public class RootStickerlist
    {
        public List<StickerlistData> Slist = new List<StickerlistData>();
    }