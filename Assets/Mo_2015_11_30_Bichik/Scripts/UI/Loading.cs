namespace Mo_2015_11_30_Bichik
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    public class Loading : MonoBehaviour
    {
        [Header("References")]
        public Slider ProgressBar;

        [HideInInspector]
        public AsyncOperation loadingOperation;

        private void Update()
        {
            if(ProgressBar !=null)
            {
                ProgressBar.value = loadingOperation.progress;
            }
        }

    }
}