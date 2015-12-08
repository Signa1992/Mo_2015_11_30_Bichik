namespace Mo_2015_11_30_Bichik
{
    using UnityEngine;
    using System.Collections;

    [RequireComponent(typeof(AudioSource))]
    public class BackgroundMusic : MonoBehaviour
    {
        public static BackgroundMusic Instance { get; private set; }

        public BackgroundMusic() : base()
        {
            Instance = this;
        }

        private new AudioSource audio;

        private void Awake()
        {
            audio = GetComponent(typeof(AudioSource)) as AudioSource;
        }

        public static float Volume
        {
            get
            {
                return Instance.audio.volume;
            }

            set
            {
                Instance.audio.volume = value;
            }
        }
    }
}
