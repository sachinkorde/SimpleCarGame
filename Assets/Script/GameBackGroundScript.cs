using UnityEngine;
using UnityEngine.UI;

    public class GameBackGroundScript : MonoBehaviour
    {
        public Transform boundryX, boundryY;
        [SerializeField] private float speed = 1.3f;
        [SerializeField] private float minRange = 10f;
        [SerializeField] private float maxRange = 25f;
        public Image kChristmasThemeImage;
        public Image kEasterThemeImage;
        public Image kHalloweenThemeImage;
        public Image currentImage;






        private void Update()
        {
            if(GameManager.instance != null)
            {
                if (GameManager.instance.currentState == GameManager.GameStates.GameOver)
                {
                    transform.Translate(Vector3.down * Time.deltaTime * 0);
                }
                else if (GameManager.instance.isBoost && GameManager.instance.currentState != GameManager.GameStates.GameOver && GameManager.instance.currentState == GameManager.GameStates.GameStart)
                {
                    transform.Translate(Vector3.down * Time.deltaTime * speed * GameManager.instance.boosterSpeed);
                }
                else
                {
                    transform.Translate(Vector3.down * Time.deltaTime * speed);
                }
            }
            
            //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (transform.name == "Island")
            {
                if (collision.transform.gameObject.name == "Destroyer")
                {
                    transform.position = new Vector3(Random.Range(boundryX.position.x, boundryY.position.x), Random.Range(minRange, maxRange), 1f);
                }
            }
        }
    }
