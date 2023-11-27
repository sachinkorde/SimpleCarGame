using DG.Tweening;
using System.Collections;
using UnityEngine;


    public class GamePlay : MonoBehaviour
    {
        public static GamePlay instance;

        public Vector3 spawnPosCandies;
        public Vector3 spawnPosCandies1;
        public Vector3 spawnPosEneies1;
        public Vector3 spawnPosEneies;
        public Vector3 spawnPosGameComponents;

        public Transform leftLane, midLane, rightLane;
        public Transform leftLaneEnemyGO, midLaneEnemyGO, rightLaneEnemyGO;
        public Transform leftLaneGems, midLaneGems, rightLaneGems;
        public Transform gemsParentMagnet, gemsParent2xPowerUp, enemiesParent, enemiesParent1, gameComponentsParent; //gemsParent, gemsParent1,

        //public GameObject debugger;

        float timeCount = 0;
        [SerializeField] private float maxRangeSpawn = 4.5f;
        [SerializeField] private float leftMaxLane = -1.65f;
        [SerializeField] private float midMaxLane = 0f;
        [SerializeField] private float rightMaxLane = 1.65f;
        [SerializeField] private float yPosLane = 10f;
        [SerializeField] private float candiesYPosLane = 1.2f;

        [SerializeField] private int targetCounter = 5;
        int howmanyEnemySpawn, whichLane, isSpawnEnemy;
        int counterForLane = 0;
        int prevIndexEnemy1 = 0, prevIndexEnemy2 = 0;
        int prevIndexGameComponent = 0;
        int prevIndexGems = 0;
        public int firstCount = 0;
        public int intialSpawnCounter = 0;

        public bool isEnemyActivated = false;
        public bool isSpawnPowerUp = false;

        private void Start()
        {
            instance = this;
            firstCount = 0;
            intialSpawnCounter = 0;
            targetCounter = Random.Range(3, 5);
        }

        private void OnEnable()
        {
            if (Screen.width < 1350)
            {
                leftLane.localPosition = new Vector3(leftMaxLane, yPosLane, 0f);
                midLane.localPosition = new Vector3(midMaxLane, yPosLane, 0f);
                rightLane.localPosition = new Vector3(rightMaxLane, yPosLane, 0f);
                leftLaneGems.localPosition = new Vector3(leftMaxLane, yPosLane, 0f);
                midLaneGems.localPosition = new Vector3(midMaxLane, yPosLane, 0f);
                rightLaneGems.localPosition = new Vector3(rightMaxLane, yPosLane, 0f);
            }
        }

        public void SpawningWithRandom()
        {
            targetCounter = Random.Range(3, 5);
            if (intialSpawnCounter < targetCounter)
            {
                StopCoroutine(ReadyToSpawnBoosterGems());
                StartCoroutine(ReadyToSpawnBoosterGems());
                intialSpawnCounter++;
            }
            else
            {
                //if (GameManager.instance.is2xPower)
                //{
                //    StopCoroutine(ReadyToSpawn2xGems());
                //    StartCoroutine(ReadyToSpawn2xGems());
                //}
                //else
                if (GameManager.instance.isBoost)
                {
                    StopCoroutine(ReadyToSpawnBoosterGems());
                    StartCoroutine(ReadyToSpawnBoosterGems());
                }
                else if (GameManager.instance.isMagnetPower)
                {
                    StopCoroutine(ReadyToSpawnMagnetGems());
                    StartCoroutine(ReadyToSpawnMagnetGems());
                }
                else
                {
                    StopCoroutine(ReadyToSpawnGems());
                    StartCoroutine(ReadyToSpawnGems());
                }
            }
        }

        IEnumerator ReadyToSpawnGems()
        {
            if (firstCount == 0)
            {
                GameManager.instance.ResumeGame();
                yield return new WaitForSeconds(3f);
                BoosterPowerSpawning();
                firstCount++;
                intialSpawnCounter++;
            }
            else
            {
                float waitTime = 0.3f;
                if (Screen.width > 1350)
                {
                    waitTime = 0.75f;
                }
                yield return new WaitForSeconds(waitTime);
                SpawningHereFun();
            }
        }

        public void LoadGamePlay()
        {
            if (GameManager.instance.currentState == GameManager.GameStates.GameStart)
            {
                UnityEngine.Resources.UnloadUnusedAssets();
                StartCoroutine(ReadyToSpawnGems());
                timeCount = 0;
            }
        }

        IEnumerator ReadyToSpawnMagnetGems()
        {
            float waitTime = 0.3f;
            if (Screen.width > 1350)
            {
                waitTime = 0.55f;
            }
            yield return new WaitForSeconds(waitTime);
            MagnetPowerSpawning();
        }

        IEnumerator ReadyToSpawnBoosterGems()
        {
            float waitTime = 0.3f;
            if (Screen.width > 1350)
            {
                waitTime = 0.5f;
            }
            yield return new WaitForSeconds(waitTime);
            BoosterPowerSpawning();
        }

        //IEnumerator ReadyToSpawn2xGems()
        //{
        //    float waitTime = 0.3f;
        //    if (Screen.width > 1350)
        //    {
        //        waitTime = 0.55f;
        //    }
        //    yield return new WaitForSeconds(waitTime);
        //    SpawningIn2xPowerUp();
        //}

        private void Update()
        {
            PowerUpCounter();
            if (GameManager.instance.currentState == GameManager.GameStates.GameOver)
            {
                StopAllCoroutines();
            }
        }

        public void PowerUpCounter()
        {
            if (GameManager.instance.currentState == GameManager.GameStates.GameStart)
            {
                if (Time.timeScale == 1)
                {
                    timeCount += Time.deltaTime;

                    //Debug.Log(timeCount + "   time count");
                }
            }
            else
            {
                timeCount = 0;
            }
        }

        #region New SpawnLogic
        public int UniqueRandomSpawnGemsInt(int min, int max, int prevVal)
        {
            int val = Random.Range(min, max);
            if (prevVal == val)
            {
                if (prevVal == 1)
                {
                    prevVal++;
                    return prevVal;
                }
                else
                {
                    if (prevVal == 0)
                    {
                        prevVal++;
                        return prevVal;
                    }
                    prevVal--;
                    return prevVal;
                }
            }
            return val;
        }

        public int UniqueRandomSpawnEnemyInt(int min, int max, int prevVal)
        {
            int val = Random.Range(min, max);
            if (prevVal == val)
            {
                if (prevVal == 1)
                {
                    prevVal++;
                    return prevVal;
                }
                else
                {
                    if (prevVal == 0)
                    {
                        prevVal++;
                        return prevVal;
                    }
                    prevVal--;
                    return prevVal;
                }
            }
            return val;
        }

        public int UniqueRandomSpawnPowerUpInt(int min, int max, int prevVal)
        {
            int val = Random.Range(min, max);
            if (prevVal == val)
            {
                if (prevVal == 1)
                {
                    prevVal++;
                    return prevVal;
                }
                else
                {
                    if (prevVal == 0)
                    {
                        prevVal++;
                        return prevVal;
                    }
                    prevVal--;
                    return prevVal;
                }
            }
            return val;
        }

        int GetIndexForGems()
        {
            int x = Random.Range(0, 3);

            if (whichLane == x)
            {
                counterForLane++;
                if (counterForLane < 2)
                {
                    return x;
                }
                else
                {
                    if (counterForLane > 3)
                    {
                        counterForLane = 0;
                    }
                    if (x == 0)
                    {
                        x++;
                        return x;
                    }
                    else
                    {
                        x--;
                        return x;
                    }
                }
            }
            else
            {
                return x;
            }
        }

        #region funCtions for Active GO
        public void GemsAvtive(int index, Transform parent)
        {
            /*if (GameManager.instance.isBoost)
            {
                spawnPosCandies = new Vector3(spawnPosCandies.x, Random.Range(spawnPosCandies.y + 3, spawnPosCandies.y + 6), spawnPosCandies.z);
            }*/
            GameObject tmpcandies = parent.GetChild(index).gameObject;
            //CoinsParentScript cps = tmpcandies.GetComponent<CoinsParentScript>();
            CoinParent2xPowerUp cps = tmpcandies.GetComponent<CoinParent2xPowerUp>();
            tmpcandies.SetActive(true);
            for (int i = 0; i < tmpcandies.transform.GetChild(0).childCount; i++)
            {
                tmpcandies.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                tmpcandies.transform.GetChild(0).GetChild(i).GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                tmpcandies.transform.GetChild(0).GetChild(i).localPosition = new Vector3(0, cps.initPosLeftCoinTransform.localPosition.y - (i * candiesYPosLane), 0);
                tmpcandies.transform.GetChild(0).GetChild(i).GetChild(0).localPosition = Vector3.zero;
            }

            for (int i = 0; i < tmpcandies.transform.GetChild(1).childCount; i++)
            {
                tmpcandies.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
                tmpcandies.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                tmpcandies.transform.GetChild(1).GetChild(i).localPosition = new Vector3(0, cps.initPosLeftCoinTransform.localPosition.y - (i * candiesYPosLane), 0);
                tmpcandies.transform.GetChild(1).GetChild(i).GetChild(0).localPosition = Vector3.zero;
            }
            tmpcandies.transform.GetChild(0).transform.position = cps.initPosLeftChildTransform.position;
            tmpcandies.transform.localPosition = spawnPosCandies;
        }

        public void EnemyActive(Transform parent, int index, int index2 = 0, Transform parent2 = null, int numberOfEnemies = 1)
        {
            if (isEnemyActivated)
            {
                return;
            }
            if (numberOfEnemies == 1)
            {
                GameObject tmpEnemy = parent.GetChild(index).gameObject;
                tmpEnemy.SetActive(true);
                tmpEnemy.transform.position = new Vector3(spawnPosEneies.x, Random.Range(spawnPosEneies.y + 3f, spawnPosEneies.y + 5f), spawnPosEneies.z);
                GamePlay.instance.isEnemyActivated = true;
            }
            else if (numberOfEnemies == 2)
            {
                GameObject tmpEnemy = parent.GetChild(index).gameObject;
                tmpEnemy.SetActive(true);
                tmpEnemy.transform.position = new Vector3(spawnPosEneies.x, Random.Range(spawnPosEneies.y, spawnPosEneies.y + 2.5f), spawnPosEneies.z);

                GameObject tmpEnemy1 = parent2.GetChild(index2).gameObject;
                tmpEnemy1.SetActive(true);
                tmpEnemy1.transform.position = new Vector3(spawnPosEneies1.x, Random.Range(tmpEnemy.transform.position.y + 3f, tmpEnemy.transform.position.y + 5f), spawnPosEneies1.z);
                GamePlay.instance.isEnemyActivated = true;
            }
        }

        public void PowerUpActive(int index, Transform parent)
        {
            GameObject tmpGameCom = parent.GetChild(index).gameObject;
            tmpGameCom.SetActive(true);
            tmpGameCom.transform.position = new Vector3(spawnPosGameComponents.x, Random.Range(spawnPosGameComponents.y, spawnPosGameComponents.y + maxRangeSpawn), spawnPosGameComponents.z);
            timeCount = 0;
        }
        #endregion


        void SetSpawningPosition(int lane)
        {
            switch (lane)
            {
                case 0:
                    spawnPosCandies = leftLaneGems.localPosition; break;
                case 1:
                    spawnPosCandies = midLaneGems.localPosition; break;
                case 2:
                    spawnPosCandies = rightLaneGems.localPosition; break;
                default:
                    break;
            }
        }

        public void SpawningHereFun()
        {
            whichLane = GetIndexForGems();

            SetSpawningPosition(whichLane);

            prevIndexGems = UniqueRandomSpawnGemsInt(0, gemsParent2xPowerUp.childCount, prevIndexGems);
            GemsAvtive(prevIndexGems, gemsParent2xPowerUp);

            isSpawnEnemy = Random.Range(0, 10);
            if (isSpawnEnemy > 1)
            {
                howmanyEnemySpawn = Random.Range(1, 3);
                if (howmanyEnemySpawn == 1)
                {
                    if (whichLane == 0)
                    {
                        spawnPosEneies = rightLane.position;
                        spawnPosGameComponents = midLane.position;
                    }
                    if (whichLane == 1)
                    {
                        spawnPosEneies = leftLane.position;
                        spawnPosGameComponents = rightLane.position;
                    }
                    if (whichLane == 2)
                    {
                        spawnPosEneies = midLane.position;
                        spawnPosGameComponents = leftLane.position;
                    }

                    prevIndexEnemy1 = UniqueRandomSpawnEnemyInt(0, enemiesParent.childCount, prevIndexEnemy1);
                    EnemyActive(enemiesParent, prevIndexEnemy1);
                    if (timeCount > 10)
                    {
                        if (!isSpawnPowerUp)
                        {
                            prevIndexGameComponent = UniqueRandomSpawnPowerUpInt(0, gameComponentsParent.childCount, prevIndexGameComponent);
                            PowerUpActive(prevIndexGameComponent, gameComponentsParent);
                        }
                        else
                        {
                            timeCount = 7;
                        }
                    }
                }
                else if (howmanyEnemySpawn == 2)
                {
                    if (whichLane == 0)
                    {
                        spawnPosEneies = rightLane.position;
                        spawnPosEneies1 = midLaneEnemyGO.position;
                    }
                    if (whichLane == 1)
                    {
                        spawnPosEneies = leftLane.position;
                        spawnPosEneies1 = rightLaneEnemyGO.position;
                    }
                    if (whichLane == 2)
                    {
                        spawnPosEneies = midLane.position;
                        spawnPosEneies1 = leftLaneEnemyGO.position;
                    }
                    prevIndexEnemy1 = UniqueRandomSpawnEnemyInt(0, enemiesParent.childCount, prevIndexEnemy1);
                    prevIndexEnemy2 = UniqueRandomSpawnEnemyInt(0, enemiesParent1.childCount, prevIndexEnemy2);
                    EnemyActive(enemiesParent, prevIndexEnemy1, prevIndexEnemy2, enemiesParent1, howmanyEnemySpawn);

                    if (timeCount > 15)
                    {
                        timeCount = 7f;
                    }
                }
            }
        }

        //public void SpawningIn2xPowerUp()
        //{
        //    whichLane = GetIndexForGems();
        //    if (whichLane == 0)
        //    {
        //        spawnPosCandies = leftLaneGems.localPosition;
        //    }

        //    if (whichLane == 1)
        //    {
        //        spawnPosCandies = midLaneGems.localPosition;
        //    }

        //    if (whichLane == 2)
        //    {
        //        spawnPosCandies = rightLaneGems.localPosition;
        //    }

        //    prevIndexGems = UniqueRandomSpawnGemsInt(0, gemsParent2xPowerUp.childCount, prevIndexGems);
        //    GameObject tmpcandies = gemsParent2xPowerUp.GetChild(prevIndexGems).gameObject;
        //    CoinParent2xPowerUp cps = tmpcandies.GetComponent<CoinParent2xPowerUp>();
        //    tmpcandies.SetActive(true);
        //    for (int i = 0; i < tmpcandies.transform.GetChild(0).childCount; i++)
        //    {
        //        tmpcandies.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
        //        tmpcandies.transform.GetChild(0).GetChild(i).GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        //        tmpcandies.transform.GetChild(0).GetChild(i).localPosition = new Vector3(cps.initPosLeftCoinTransform.localPosition.x, cps.initPosLeftCoinTransform.localPosition.y - (i * candiesYPosLane), 0);
        //    }

        //    for (int i = 0; i < tmpcandies.transform.GetChild(1).childCount; i++)
        //    {
        //        tmpcandies.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
        //        tmpcandies.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        //        tmpcandies.transform.GetChild(1).GetChild(i).localPosition = new Vector3(cps.initPosLeftCoinTransform.localPosition.x, cps.initPosLeftCoinTransform.localPosition.y - (i * candiesYPosLane), 0);
        //    }
        //    tmpcandies.transform.GetChild(0).transform.position = cps.initPosLeftChildTransform.position;
        //    tmpcandies.transform.GetChild(1).transform.position = cps.initPosLeftChildTransform.position;
        //    tmpcandies.transform.localPosition = spawnPosCandies;

        //    isSpawnEnemy = Random.Range(0, 10);
        //    if (isSpawnEnemy > 1)
        //    {
        //        howmanyEnemySpawn = Random.Range(0,3);
        //        if (howmanyEnemySpawn == 1)
        //        {
        //            if (whichLane == 0)
        //            {
        //                spawnPosEneies = rightLane.position;
        //            }
        //            if (whichLane == 1)
        //            {
        //                spawnPosEneies = leftLane.position;
        //            }
        //            if (whichLane == 2)
        //            {
        //                spawnPosEneies = midLane.position;
        //            }
        //            prevIndexEnemy1 = UniqueRandomSpawnEnemyInt(0, enemiesParent.childCount, prevIndexEnemy1);
        //            EnemyActive(enemiesParent, prevIndexEnemy1);
        //        }
        //        else if (howmanyEnemySpawn == 2)
        //        {
        //            if (whichLane == 0)
        //            {
        //                spawnPosEneies = rightLane.position;
        //                spawnPosEneies1 = midLaneEnemyGO.position;
        //            }
        //            if (whichLane == 1)
        //            {
        //                spawnPosEneies = leftLane.position;
        //                spawnPosEneies1 = rightLaneEnemyGO.position;
        //            }
        //            if (whichLane == 2)
        //            {
        //                spawnPosEneies = midLane.position;
        //                spawnPosEneies1 = leftLaneEnemyGO.position;
        //            }
        //            prevIndexEnemy1 = UniqueRandomSpawnEnemyInt(0, enemiesParent.childCount, prevIndexEnemy1);
        //            prevIndexEnemy2 = UniqueRandomSpawnEnemyInt(0, enemiesParent1.childCount, prevIndexEnemy2);

        //            EnemyActive(enemiesParent, prevIndexEnemy1, prevIndexEnemy2, enemiesParent1, howmanyEnemySpawn);
        //        }
        //    }
        //}

        public void MagnetPowerSpawning()
        {
            whichLane = GetIndexForGems();
            if (whichLane == 0)
            {
                spawnPosCandies = leftLaneGems.position;
                spawnPosCandies1 = midLaneGems.position;
            }
            if (whichLane == 1)
            {
                spawnPosCandies = rightLaneGems.position;
                spawnPosCandies1 = leftLaneGems.position;
            }
            if (whichLane == 2)
            {
                spawnPosCandies = rightLaneGems.position;
                spawnPosCandies1 = midLaneGems.position;
            }

            prevIndexGems = UniqueRandomSpawnGemsInt(0, gemsParentMagnet.childCount, prevIndexGems);
            GemsAvtive(prevIndexGems, gemsParent2xPowerUp);

            GameObject tmpcandies1 = gemsParentMagnet.GetChild(prevIndexGems).gameObject;
            //CoinParent2xPowerUp cps1 = tmpcandies1.GetComponent<CoinParent2xPowerUp>();
            CoinParentForActivateMagnet cps1 = tmpcandies1.GetComponent<CoinParentForActivateMagnet>();
            tmpcandies1.SetActive(true);

            for (int i = 0; i < tmpcandies1.transform.GetChild(0).childCount; i++)
            {
                tmpcandies1.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                tmpcandies1.transform.GetChild(0).GetChild(i).GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                tmpcandies1.transform.GetChild(0).GetChild(i).localPosition = new Vector3(0, cps1.initPosLeftCoinTransform.localPosition.y - (i * candiesYPosLane), tmpcandies1.transform.GetChild(0).GetChild(i).localPosition.z);
                tmpcandies1.transform.GetChild(0).GetChild(i).GetChild(0).localPosition = Vector3.zero;
            }
            tmpcandies1.transform.GetChild(0).transform.position = cps1.initPosLeftChildTransform.position;
            tmpcandies1.transform.localPosition = spawnPosCandies1;
            isSpawnEnemy = Random.Range(0, 10);

            if (isSpawnEnemy > 2)
            {
                if (whichLane == 0)
                {
                    spawnPosEneies = rightLane.localPosition;
                }
                if (whichLane == 1)
                {
                    spawnPosEneies = midLane.localPosition;
                }
                if (whichLane == 2)
                {
                    spawnPosEneies = leftLane.localPosition;
                }
                prevIndexEnemy1 = UniqueRandomSpawnEnemyInt(0, enemiesParent.childCount, prevIndexEnemy1);
                EnemyActive(enemiesParent, prevIndexEnemy1);
            }
        }

        public void BoosterPowerSpawning()
        {
            whichLane = GetIndexForGems();
            if (whichLane == 0)
            {
                spawnPosCandies = leftLaneGems.position;
            }

            if (whichLane == 1)
            {
                spawnPosCandies = midLaneGems.position;
            }

            if (whichLane == 2)
            {
                spawnPosCandies = rightLaneGems.position;
            }

            prevIndexGems = UniqueRandomSpawnGemsInt(0, gemsParent2xPowerUp.childCount, prevIndexGems);
            GemsAvtive(prevIndexGems, gemsParent2xPowerUp);
        }
        #endregion

        #region Debugger spawn
        public void SpawnTwoX()
        {
            whichLane = Random.Range(0, 2);
            if (whichLane == 0)
            {
                spawnPosGameComponents = leftLane.position;
            }
            else if (whichLane == 1)
            {
                spawnPosGameComponents = midLane.position;
            }
            else if (whichLane == 2)
            {
                spawnPosGameComponents = rightLane.position;
            }

            GameObject tmpGameCom = gameComponentsParent.GetChild(0).gameObject;
            tmpGameCom.SetActive(true);
            tmpGameCom.transform.position = spawnPosGameComponents;
        }

        public void SpawnBooster()
        {
            whichLane = Random.Range(0, 2);
            if (whichLane == 0)
            {
                spawnPosGameComponents = leftLane.position;
            }
            else if (whichLane == 1)
            {
                spawnPosGameComponents = midLane.position;
            }
            else if (whichLane == 2)
            {
                spawnPosGameComponents = rightLane.position;
            }

            GameObject tmpGameCom = gameComponentsParent.GetChild(1).gameObject;
            tmpGameCom.SetActive(true);
            tmpGameCom.transform.position = spawnPosGameComponents;
        }

        public void SpawnMagnet()
        {
            whichLane = 1;//Random.Range(0, 2);
            if (whichLane == 0)
            {
                spawnPosGameComponents = leftLane.position;
            }
            else if (whichLane == 1)
            {
                spawnPosGameComponents = midLane.position;
            }
            else if (whichLane == 2)
            {
                spawnPosGameComponents = rightLane.position;
            }

            GameObject tmpGameCom = gameComponentsParent.GetChild(2).gameObject;
            tmpGameCom.SetActive(true);
            tmpGameCom.transform.position = spawnPosGameComponents;
        }

        public void SpawnShield()
        {
            whichLane = Random.Range(0, 2);
            if (whichLane == 0)
            {
                spawnPosGameComponents = leftLane.position;
            }
            else if (whichLane == 1)
            {
                spawnPosGameComponents = midLane.position;
            }
            else if (whichLane == 2)
            {
                spawnPosGameComponents = rightLane.position;
            }

            GameObject tmpGameCom = gameComponentsParent.GetChild(3).gameObject;
            tmpGameCom.SetActive(true);
            tmpGameCom.transform.position = spawnPosGameComponents;
        }

        //public void DebugerOff()
        //{
        //    debugger.transform.DOScale(0f, 0.75f);
        //}

        //public void DebugerOn()
        //{
        //    debugger.transform.DOScale(1, 0.75f);
        //}
        #endregion

        public void PreRestartGame()
        {
            isSpawnPowerUp = false;
            isEnemyActivated = false;
            timeCount = 0;

            for (int i = 0; i < gemsParentMagnet.childCount; i++)
            {
                gemsParentMagnet.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < gemsParent2xPowerUp.childCount; i++)
            {
                gemsParent2xPowerUp.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < enemiesParent.childCount; i++)
            {
                enemiesParent.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < enemiesParent1.childCount; i++)
            {
                enemiesParent1.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < gameComponentsParent.childCount; i++)
            {
                gameComponentsParent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
