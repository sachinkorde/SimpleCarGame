using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CkGO
{
    public class UnloadCKGO : MonoBehaviour
    {
        void Start()
        {
            //AssetBundleManager.instance.UnloadBundle();
            SoundManager.DestroyInstance();
            Resources.UnloadUnusedAssets();
        }
    }
}