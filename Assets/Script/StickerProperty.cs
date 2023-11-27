using UnityEngine;
using UnityEngine.UI;

    public class StickerProperty : MonoBehaviour
    {
        public static StickerProperty instance;
        public bool isLocked = true;
        public GameObject lockImage;
        public int id;
        [SerializeField] private string StickerName = "StickerName";

        //Locked : PlayerPref is false.
        //Unlocked : PlayerPref is true.
        void Start()
        {
            instance = this;

            if (GetStickerStatus())
            {
                isLocked = true;
                if (gameObject.GetComponent<Button>() != null)
                {
                    gameObject.GetComponent<Button>().enabled = true;
                }

                if (lockImage != null)
                {
                    lockImage.SetActive(true);
                }
            }
            else
            {
                isLocked = false;
                if (gameObject.GetComponent<Button>() != null)
                {
                    gameObject.GetComponent<Button>().enabled = false;
                }
                if (lockImage != null)
                {
                    lockImage.SetActive(false);
                }
            }
        }

        public bool GetStickerStatus()
        {
            Debug.Log("GetStickerStatus");
            Debug.Log("Sticker Name : " + gameObject.name);
            //string lockstatus = PBPlayerPrefs.GetBool(gameObject.name + ".png", false) ? "Unlocked" : "Locked";  
            //Debug.Log(gameObject.name + " is " + lockstatus);

            //if (!PBPlayerPrefs.GetBool(gameObject.name + ".png", false))
            {
                return true;
            }
            //else
            {
                return false;
            }
        }

        public void SetStickerUnlock()
        {
            isLocked = false;
            if (lockImage != null)
            {
                lockImage.SetActive(false);
                //PBPlayerPrefs.SetBool(gameObject.name + ".png", true);
            }
        }

        public void PopUpLock()
        {
            if (this.transform.GetComponent<StickerProperty>().isLocked)
            {
                StickerGame.instance.PopUpAnim();
            }
        }
    }
