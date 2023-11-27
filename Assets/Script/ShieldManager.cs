using UnityEngine;


    public class ShieldManager : MonoBehaviour
    {
        [SerializeField] private string flipHiAnim = "flip hi";
        [SerializeField] private string newIdleFly = "idle_fly";

        private void Update()
        {
            if (GameManager.instance.isBoost && GameManager.instance.currentState != GameManager.GameStates.GameOver)
            {
                transform.Translate(Vector3.down * Time.deltaTime * GameManager.instance.objectSpeed * GameManager.instance.boosterSpeed);
            }
            else if (GameManager.instance.currentState == GameManager.GameStates.GameOver)
            {
                transform.Translate(Vector3.down * Time.deltaTime * 0);
            }
            else
            {
                transform.Translate(Vector3.down * Time.deltaTime * GameManager.instance.objectSpeed);
            }
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (gameObject.name == "Shield")
            {
                if (collision.gameObject.name == "Player")
                {
                    gameObject.SetActive(false);
                    GameManager.instance.isShield = true;
                    GameManager.instance.PowerUpsStart(GameManager.instance.isShield);

                    GameManager.instance.shieldTitleImage.SetTrigger("startTitleAnim");
                    //SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[1], 1);
                    //SoundManager.instance.PlayOneShot(gameAudioClip.collect_powerup);
                    SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[25], 1);
                    GameManager.instance._skeletonAnimPlayer.AnimationState.SetAnimation(0, flipHiAnim, false);
                    GameManager.instance._skeletonAnimPlayer.AnimationState.AddAnimation(0, newIdleFly, true, 0);
                    transform.position = new Vector3(GamePlay.instance.spawnPosGameComponents.x, GamePlay.instance.spawnPosGameComponents.y, 0);
                }

                if (collision.gameObject.name == "Destroyer")
                {
                    gameObject.SetActive(false);
                    transform.position = new Vector3(GamePlay.instance.spawnPosGameComponents.x, GamePlay.instance.spawnPosGameComponents.y, 0);
                }

                if (collision.transform.gameObject.name == "SoundTrigger")
                {
                    SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[17], 1);
                    //SoundManager.instance.PlayOneShot(gameAudioClip.power_up_entry);
                }
            }
        }
    }

