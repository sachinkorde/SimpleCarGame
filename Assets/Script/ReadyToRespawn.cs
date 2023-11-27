using UnityEngine;


    public class ReadyToRespawn : MonoBehaviour
    {
        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.gameObject.name == "Destroyer" || collision.gameObject.name == "Player")
            {
                gameObject.transform.parent.parent.parent.gameObject.SetActive(false);
                gameObject.transform.parent.parent.parent.localPosition = GamePlay.instance.spawnPosCandies;
            }
        }
    }
