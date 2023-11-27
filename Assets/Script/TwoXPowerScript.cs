using UnityEngine;


    public class TwoXPowerScript : MonoBehaviour
    {
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
            if (gameObject.name == "2xPower")
            {
                if (collision.gameObject.name == "Player")
                {
                    //SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[1], 1);
                    //SoundManager.instance.PlayOneShot(gameAudioClip.collect_powerup);
                    SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[22], 1);
                    GameManager.instance._skeletonAnimPlayer.AnimationState.SetAnimation(0, "music", false);
                    GameManager.instance._skeletonAnimPlayer.AnimationState.AddAnimation(0, "idle_fly", true, 1f);
                    GameManager.instance.is2xPower = true;
                    GameManager.instance.PowerUpsStart(GameManager.instance.is2xPower);

                    GameManager.instance.twoXTitleImage.SetTrigger("startTitleAnim");
                    gameObject.SetActive(false);
                    transform.position = new Vector3(GamePlay.instance.spawnPosGameComponents.x, GamePlay.instance.spawnPosGameComponents.y, 0);
                }

                if (collision.transform.name == "Destroyer")
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
