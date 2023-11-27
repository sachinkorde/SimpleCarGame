using CkGO;
using UnityEngine;
using Spine.Unity;

public class BoosterManager : MonoBehaviour
{
    [SerializeField] private float gameOverSpeed = 0f;

    private void Update()
    {
        if (GameManager.instance.currentState != GameManager.GameStates.GameOver)
        {
            transform.Translate(Vector3.down * Time.deltaTime * GameManager.instance.objectSpeed);
        }
        else if (GameManager.instance.currentState == GameManager.GameStates.GameOver)
        {
            transform.Translate(Vector3.down * Time.deltaTime * gameOverSpeed);
        }
        else
        {
            transform.Translate(Vector3.down * Time.deltaTime * GameManager.instance.objectSpeed);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.gameObject.name == "Player")
        {
            GameManager.instance.isBoost = true;
            GameManager.instance.PowerUpsStart(GameManager.instance.isBoost);
            GameManager.instance.boosterTitleImage.SetTrigger("startTitleAnim");
            //SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[1], 1f);
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[24], 1);
            //SoundManager.instance.PlayOneShot(gameAudioClip.camera_click);
            gameObject.SetActive(false);
            GameManager.instance._skeletonAnimPlayer.timeScale = 1.5f;
            transform.position = new Vector3(GamePlay.instance.spawnPosGameComponents.x, GamePlay.instance.spawnPosGameComponents.y, 0);
        }

        if (collision.transform.gameObject.name == "Destroyer")
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
