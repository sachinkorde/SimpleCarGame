using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


    public class LoadGame : MonoBehaviour
    {
        [SerializeField] private string game = "ckgoGame";

        public void OnLoadButtonClick()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            SceneManager.LoadScene(game);
        }

        public void OnMouseDownEnter(Button buttonDown)
        {
            buttonDown.transform.localScale = new Vector2(1.15f, 1.15f);
        }

        public void OnMouseDownExit(Button buttonExit)
        {
            buttonExit.transform.localScale = new Vector2(1f, 1f);
        }
    }
