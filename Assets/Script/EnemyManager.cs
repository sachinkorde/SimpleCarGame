using UnityEngine;

    public class EnemyManager : MonoBehaviour
    {
        public float extraSpeed = 0.1f;

        private void Update()
        {
            if (GameManager.instance.currentState == GameManager.GameStates.GameOver && GameManager.instance.isGameOver)
            {
                transform.Translate(Vector3.zero);
            }
            else if (GameManager.instance.isBoost && GameManager.instance.currentState != GameManager.GameStates.GameOver && GameManager.instance.currentState == GameManager.GameStates.GameStart)
            {
                transform.Translate(Vector3.down * Time.deltaTime * GameManager.instance.objectSpeed * GameManager.instance.boosterSpeed);
            }
            else
            {
                if (gameObject.name == "spaceship")
                {
                    transform.Translate(Vector3.down * Time.deltaTime * GameManager.instance.objectSpeed * extraSpeed);
                }

                if (gameObject.name == "asteroid")
                {
                    transform.Translate(Vector3.down * Time.deltaTime * GameManager.instance.objectSpeed * extraSpeed);
                }
                transform.Translate(Vector3.down * Time.deltaTime * GameManager.instance.objectSpeed);
            }
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.gameObject.name == "MidScreenTrigger")
            {
                GamePlay.instance.isEnemyActivated = false;

                if (gameObject.name == "spaceship")
                {
                    SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[13], 1f);
                    //SoundManager.instance.PlayOneShot(gameAudioClip.spaceship_zoom_out);
                }
            }

            if (collision.transform.gameObject.name == "Destroyer")
            {
                gameObject.SetActive(false);
                transform.position = new Vector3(GamePlay.instance.spawnPosEneies.x, GamePlay.instance.spawnPosEneies.y, 0);
                GamePlay.instance.isEnemyActivated = false;
            }

            if (collision.transform.gameObject.name == "SoundTrigger")
            {
                if (gameObject.name == "spaceship")
                {
                    SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[2], 1f);
                    //SoundManager.instance.PlayOneShot(gameAudioClip.collect_powerup);
                }

                if (gameObject.name == "asteroid")
                {
                    SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[12], 1f);
                    //SoundManager.instance.PlayOneShot(gameAudioClip.meteor);
                }

                if (gameObject.name == "Cloud")
                {
                    SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[26], 1f);
                    //SoundManager.instance.PlayOneShot(gameAudioClip.meteor);
                }
            }

            //return;
            if (GameManager.instance.isShield || GameManager.instance.isBoost)
            {
                if (collision.transform.gameObject.name == "Player")
                {
                    Physics2D.IgnoreCollision(collision.transform.GetComponent<BoxCollider2D>(), transform.GetComponent<BoxCollider2D>());
                }

                if (collision.transform.name == "PlayerShield")
                {
                    gameObject.SetActive(false);
                    transform.position = new Vector3(GamePlay.instance.spawnPosEneies.x, GamePlay.instance.spawnPosEneies.y, 0);
                }
            }
            else if (collision.transform.gameObject.name == "Player")
            {
                SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[18], 1f);
                //SoundManager.instance.PlayOneShot(gameAudioClip.boing);
                GamePlay.instance.isEnemyActivated = false;
                GameManager.instance.meter.SetTrigger("idle");
                GameManager.instance.meter.gameObject.SetActive(false);
                GameManager.instance.OnEndPowerUpTitlesAnim();
                GameManager.instance.currentState = GameManager.GameStates.GameOver;

                Debug.Log(PlayerPrefsManager.GemsFillAmount);

                if (gameObject.name == "spaceship")
                {
                    PlayerDiedAnimations.instance.spaceShipAnimParent.transform.position = gameObject.transform.position;
                    PlayerDiedAnimations.instance.SpaceshipDiedAnim();
                    gameObject.SetActive(false);
                }

                if (gameObject.name == "asteroid")
                {
                    PlayerDiedAnimations.instance.asteroidBurstParent.transform.position = gameObject.transform.position;
                    PlayerDiedAnimations.instance.AsteroidBurstEffect();
                    gameObject.SetActive(false);
                }

                if (gameObject.name == "Cloud")
                {
                    PlayerDiedAnimations.instance.ElectricSparkEffectPlayer();
                }
            }
        }
    }