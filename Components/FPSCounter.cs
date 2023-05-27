using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    // Unityはデフォルトでは垂直同期しているので、
    // PC上ではフレームレートが144に達してもおかしくない。
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField]
        float updateInterval = 0.5f;

        float accum;
        int frames;
        float timeleft;
        float fps;

        void Update()
        {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            frames++;

            if (0 < timeleft) return;

            fps = accum / frames;
            timeleft = updateInterval;
            accum = 0;
            frames = 0;
        }

        void OnGUI()
        {
            GUILayout.Label("FPS: " + fps.ToString("f2"));
        }
    }
}
