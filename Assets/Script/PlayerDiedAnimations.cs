using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class PlayerDiedAnimations : MonoBehaviour
    {
        public static PlayerDiedAnimations instance;
        public List<GameObject> sparkDiedAnim;
        public List<GameObject> spaceShipAndAsteroidDie;
        public GameObject sparkdeadParent, spaceShipAnim, spaceShipAnimParent, spaceShipandAsteroidParent, asteroidBurstParent, asteroidBurst;

        public void Start()
        {
            instance = this;
            spaceShipAnim.SetActive(false);
            asteroidBurst.SetActive(false);
        }

        public void ElectricSparkEffectPlayer()
        {
            StartCoroutine(ElectricSparkDieAnim());
        }

        public IEnumerator ElectricSparkDieAnim()
        {
            for (int i = 0; i < sparkDiedAnim.Count; i++)
            {
                if (PlayerPrefsManager.SelectedPlayerName == sparkDiedAnim[i].name)
                {
                    sparkDiedAnim[i].SetActive(true);
                    sparkdeadParent.transform.position = GameManager.instance.player.transform.position;
                }
            }
            GameManager.instance.player.SetActive(false);
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[9], 1f);
            //SoundManager.instance.PlayOneShot(gameAudioClip.electric_spark);
            yield return new WaitForSeconds(2.2f);
        }

        public void SpaceshipDiedAnim()
        {
            StartCoroutine(SpaceShipDieIenumarator());
        }

        public IEnumerator SpaceShipDieIenumarator()
        {
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[8], 1f);
            //SoundManager.instance.PlayOneShot(gameAudioClip.collide_ship);
            spaceShipAnim.SetActive(true);
            for (int i = 0; i < spaceShipAndAsteroidDie.Count; i++)
            {
                if (PlayerPrefsManager.SelectedPlayerName == spaceShipAndAsteroidDie[i].name)
                {
                    spaceShipAndAsteroidDie[i].SetActive(true);
                    spaceShipandAsteroidParent.transform.position = GameManager.instance.player.transform.position;
                }
            }
            GameManager.instance.player.SetActive(false);
            yield return new WaitForSeconds(5);
        }

        public void AsteroidBurstEffect()
        {
            StartCoroutine(AsteroidDieAnim());
        }

        public IEnumerator AsteroidDieAnim()
        {
            asteroidBurst.SetActive(true);
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[11], 1f);
            //SoundManager.instance.PlayOneShot(gameAudioClip.asteroid_break);
            for (int i = 0; i < spaceShipAndAsteroidDie.Count; i++)
            {
                if (PlayerPrefsManager.SelectedPlayerName == spaceShipAndAsteroidDie[i].name)
                {
                    spaceShipAndAsteroidDie[i].SetActive(true);
                    spaceShipandAsteroidParent.transform.position = GameManager.instance.player.transform.position;
                    GameManager.instance.player.SetActive(false);
                }
            }
            GameManager.instance.player.SetActive(false);
            yield return new WaitForSeconds(2f);
        }

        public void PreRestartGame()
        {
            for (int i = 0; i < sparkDiedAnim.Count; i++)
            {
                sparkDiedAnim[i].SetActive(false);
            }

            for (int i = 0; i < spaceShipAndAsteroidDie.Count; i++)
            {
                spaceShipAndAsteroidDie[i].SetActive(false);
            }

            for (int i = 0; i < sparkdeadParent.transform.childCount; i++)
            {
                sparkdeadParent.transform.GetChild(i).gameObject.SetActive(false);
            }
            spaceShipAnim.SetActive(false);

            for (int i = 0; i < spaceShipandAsteroidParent.transform.childCount; i++)
            {
                spaceShipandAsteroidParent.transform.GetChild(i).gameObject.SetActive(false);
            }
            asteroidBurst.SetActive(false);
        }
    }

