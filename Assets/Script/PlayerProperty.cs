using Spine.Unity;
using UnityEngine;


public class PlayerProperty : MonoBehaviour
{
    public static PlayerProperty instance;
    public bool isLocked = true;
    public GameObject lockImage;

    //Locked : PlayerPref is true.
    //Unlocked : PlayerPref is false.

    private void Start()
    {
        instance = this;

        if (gameObject.name == "default_char")
        {
            isLocked = false;
            lockImage.SetActive(false);
            PlayerPrefs.GetString(gameObject.name + ".lock");
        }
        else
        {
            if (GetPlayerStatus())
            {
                isLocked = true;
                GetComponent<SkeletonGraphic>().color = Color.grey;
                if (lockImage != null)
                {
                    lockImage.SetActive(true);
                }
            }
            else
            {
                isLocked = false;
                if (lockImage != null)
                {
                    lockImage.SetActive(false);
                }
            }

        }
    }

    public bool GetPlayerStatus()
    {
        Debug.Log("GetPlayerStatus");
        Debug.Log("Char Name : " + gameObject.name + ".lock");
        //string lockstatus = PlayerPrefs.GetString(gameObject.name + ".lock") ? "Locked" : "Unlocked";
        //Debug.Log(gameObject.name + " is " + lockstatus);

        //if (PBPlayerPrefs.GetBool(gameObject.name + ".lock", true))
        {
            return true;
        }
        //else
        {
            return false;
        }
    }

    public void SetPlayerUnlock()
    {
        isLocked = false;
       if (lockImage != null)
       {
           lockImage.SetActive(false);
          //PBPlayerPrefs.SetBool(gameObject.name + ".lock", false);

          Debug.Log("call for unlock player");
       }
    }
}
