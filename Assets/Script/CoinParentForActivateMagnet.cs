using UnityEngine;


    public class CoinParentForActivateMagnet : MonoBehaviour
    {
        public Transform initPosLeftCoinTransform;
        public Transform initPosLeftChildTransform;
        public string CoinsParentMagnet = "CoinsParentMagnet";

        void Update()
        {
            if (GameManager.instance.isBoost && GameManager.instance.currentState != GameManager.GameStates.GameOver && GameManager.instance.currentState == GameManager.GameStates.GameStart)
            {
                this.transform.Translate(Vector3.down * Time.deltaTime * GameManager.instance.objectSpeed * GameManager.instance.boosterSpeed);
            }
            else if (GameManager.instance.currentState == GameManager.GameStates.GameOver)
            {
                this.transform.Translate(Vector3.down * Time.deltaTime * 0);
            }
            else
            {
                this.transform.Translate(Vector3.down * Time.deltaTime * GameManager.instance.objectSpeed);
            }
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);

            if (!transform.GetChild(0).GetChild(0).gameObject.activeSelf)
            {
                this.gameObject.SetActive(false);
                this.transform.position = GamePlay.instance.spawnPosCandies;
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.gameObject.name == "Destroyer")
            {
                gameObject.SetActive(false);
            }

            if (collision.transform.name == "PlayerShield")
            {
                Physics2D.IgnoreCollision(collision.transform.GetComponent<CircleCollider2D>(), this.transform.GetComponent<BoxCollider2D>());
            }

            if (collision.transform.name == "Player")
            {
                Physics2D.IgnoreCollision(collision.transform.GetComponent<BoxCollider2D>(), this.transform.GetComponent<BoxCollider2D>());
            }
        }
    }

