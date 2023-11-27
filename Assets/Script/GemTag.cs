using UnityEngine;

    
    public class GemTag : MonoBehaviour
    {
        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.gameObject.name == "MidScreenTrigger")
            {
                if (GameManager.instance.currentState != GameManager.GameStates.GameOver)
                {
                    GamePlay.instance.SpawningWithRandom();
                }
            }
        }
    }

