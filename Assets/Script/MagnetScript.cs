using UnityEngine;


    public class MagnetScript : MonoBehaviour
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
            if (collision.gameObject.name == "Player")
            {
                GameManager.instance.isMagnetPower = true;
                GameManager.instance.PowerUpsStart(GameManager.instance.isMagnetPower);
                GameManager.instance.magnetTitleImage.SetTrigger("startTitleAnim");
                //SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[1], 1);
                SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[23], 1);
                //SoundManager.instance.PlayOneShot(gameAudioClip.collect_powerup);
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
