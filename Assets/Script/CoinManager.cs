using CkGO;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private SpriteRenderer currentSprite;
    Animator currentAnimator;
    public Sprite[] easterSprite;
    public Sprite summerSprite;
    public Sprite halloweenSprite;
    public Sprite[] candySprite;

    bool isCollidewithMagnetTrigger = false;

    private void Awake()
    {
        currentSprite = GetComponent<SpriteRenderer>();
        currentAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isCollidewithMagnetTrigger)
        {
            if (Vector3.Distance(GameManager.instance.playerMagnet.transform.position, this.transform.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.playerMagnet.transform.position, 25f * Time.deltaTime);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.gameObject.name == "Player")
        {
            ScoreManager.instance.coinsCount++;
            GameManager.instance.CoinCollectionAnimFun();
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[20], 1);
            GameManager.instance.CoinMeterFun();
            GameManager.instance.ChangeAnimCoinMeterProgess();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            isCollidewithMagnetTrigger = false;
        }

        if (collision.transform.gameObject.name == "Destroyer")
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            isCollidewithMagnetTrigger = false;
        }

        if (collision.transform.name == "PlayerShield")
        {
            Physics2D.IgnoreCollision(collision.transform.GetComponent<CircleCollider2D>(), transform.GetComponent<PolygonCollider2D>());
        }

        if (collision.transform.name == "MidScreenTrigger_magnet")
        {
            if (GameManager.instance.isMagnetPower)
            {
                isCollidewithMagnetTrigger = true;
            }
        }
    }
}
