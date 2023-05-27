using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public class UnloadUnusedAssets : MonoBehaviour
    {
        void Awake()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}
