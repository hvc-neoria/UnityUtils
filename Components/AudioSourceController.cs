using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public class AudioSourceController : MonoBehaviour
    {
        AudioSource source;
        [SerializeField] AudioClip[] clips;

        void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        public void PlayClip(int index)
        {
            source.clip = clips[index];
            source.Play();
        }
    }
}
