using UnityEngine;


    public class PlayerPrefsManager : MonoBehaviour
    {
        //New test code orientation
        public static PlayerPrefsManager instance;

        private void Start()
        {
            instance = this;
        }

        private const string Orientation = "Orientation";

        public static bool IsPortrait()
        {
            return PlayerPrefs.GetInt(Orientation, 1) == 1;
        }

        public static bool HasOrientation()
        {
            return PlayerPrefs.HasKey(Orientation);
        }

        public static void SavePortrait(bool isPortrait)
        {
            PlayerPrefs.SetInt(Orientation, isPortrait ? 1 : 0);
        }
        // end orientation


        public static string SelectedPlayerName
        {
            get
            {
                return PlayerPrefs.GetString("selectedPlayer");
            }
            set
            {
                PlayerPrefs.SetString("selectedPlayer", value);
            }
        }

        public static float GemsFillAmount
        {
            get
            {
                return PlayerPrefs.GetFloat("GemsFillAmount");
            }
            set
            {
                PlayerPrefs.SetFloat("GemsFillAmount", value); ;
            }
        }

        public static int CurrentScore
        {
            get
            {
                return PlayerPrefs.GetInt("currentScore");
            }
            set
            {
                PlayerPrefs.SetInt("currentScore", value);
            }
        }

        public static int Coins
        {
            get
            {
                return PlayerPrefs.GetInt("coins");
            }
            set
            {
                PlayerPrefs.SetInt("coins", value);
            }
        }

        public static int HighScore
        {
            get
            {
                return PlayerPrefs.GetInt("highscore");
            }
            set
            {
                PlayerPrefs.SetInt("highscore", value);
            }
        }

        public static int CoinMeterScore
        {
            get
            {
                return PlayerPrefs.GetInt("CoinMeterScore");
            }
            set
            {
                PlayerPrefs.SetInt("CoinMeterScore", value);
            }
        }

        public static int CountForReward
        {
            get
            {
                return PlayerPrefs.GetInt("countforReward");
            }
            set
            {
                PlayerPrefs.SetInt("countforReward", value);
            }
        }

        public static float SetSpeed
        {
            get
            {
                return PlayerPrefs.GetFloat("SetSpeed");
            }
            set
            {
                PlayerPrefs.SetFloat("SetSpeed", value);
            }
        }

        public static float SetMeterSpeed
        {
            get
            {
                return PlayerPrefs.GetFloat("thisisMeterValue");
            }
            set
            {
                PlayerPrefs.SetFloat("thisisMeterValue", value);
            }
        }

        public static float Player_Gap
        {
            get
            {
                return PlayerPrefs.GetFloat("player_gap");
            }
            set
            {
                PlayerPrefs.SetFloat("player_gap", value);
            }
        }
    }
