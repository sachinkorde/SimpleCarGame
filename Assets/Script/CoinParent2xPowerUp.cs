using CkGO;
using UnityEngine;

public class CoinParent2xPowerUp : MonoBehaviour
{
    public Transform initPosLeftCoinTransform;
    public Transform initPosLeftChildTransform;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

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

        if (GameManager.instance.is2xPower)
        {
            transform.GetChild(0).transform.localPosition = new Vector3(-0.43f, transform.GetChild(0).transform.localPosition.y, transform.GetChild(0).transform.localPosition.z);
            transform.GetChild(1).transform.localPosition = new Vector3(0.33f, transform.GetChild(1).transform.localPosition.y, transform.GetChild(1).transform.localPosition.z);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).transform.localPosition = new Vector3(0.0f, transform.GetChild(0).transform.localPosition.y, transform.GetChild(0).transform.localPosition.z);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "PlayerShield")
        {
            Physics2D.IgnoreCollision(collision.transform.GetComponent<CircleCollider2D>(), transform.GetComponent<BoxCollider2D>());
        }

        if (collision.transform.name == "Player")
        {
            Physics2D.IgnoreCollision(collision.transform.GetComponent<BoxCollider2D>(), transform.GetComponent<BoxCollider2D>());
        }
    }
}
