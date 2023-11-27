using UnityEngine;


    public enum GameScenes
    {
        home,
        game,
    }

    public class SceneHandler : MonoBehaviour
    {
        public static SceneHandler instance;
        public GameScenes sceneToLoad;
        //public GameObject[] gamePanels;
        public GameObject spalshScreen;
        public GameObject playerCarouselScript;

        private void Start()
        {
            instance = this;
            //CloseAllPanels(0);
        }

        //public void CloseAllPanels(int index)
        //{
        //    spalshScreen.SetActive(true);
        //    for (int i = 0; i < gamePanels.Length; i++)
        //    {
        //        gamePanels[i].SetActive(false);
        //    }
        //    gamePanels[index].SetActive(true);
        //}

        public void LoadHomeScene()
        {
            sceneToLoad = GameScenes.home;
            PlayerPrefs.SetString("LoadGame", "");
            Time.timeScale = 1;
            //CloseAllPanels(0);
            playerCarouselScript.GetComponent<PlayerCarousel>().enabled = true;
            HomeSceneManager.instance.HomeSceneToLoad();
            PlayerCarousel.instance.PlayerCarouselAnimation();
        }

        public void LoadGameScene()
        {
            sceneToLoad = GameScenes.game;
            PlayerPrefs.SetString("LoadGame", "Gamescene");
            playerCarouselScript.GetComponent<PlayerCarousel>().enabled = false;
            //CloseAllPanels(1);
        }
    }
