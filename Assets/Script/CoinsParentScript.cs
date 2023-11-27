using UnityEngine;


    public class CoinsParentScript : MonoBehaviour
    {
        public Transform initPosLeftCoinTransform;
        public Transform initPosLeftChildTransform;

        void Update()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            if (GameManager.instance.currentState == GameManager.GameStates.GameOver)
            {
                transform.Translate(Vector3.down * Time.deltaTime * 0);
            }
            else if (GameManager.instance.isBoost && GameManager.instance.currentState != GameManager.GameStates.GameOver && GameManager.instance.currentState == GameManager.GameStates.GameStart)
            {
                transform.Translate(Vector3.down * Time.deltaTime * GameManager.instance.objectSpeed * GameManager.instance.boosterSpeed);
            }
            else
            {
                transform.Translate(Vector3.down * Time.deltaTime * GameManager.instance.objectSpeed);
            }
        }
    }
